
//孙高勇封装的对工作流的操作的js脚本
//1.0.1 sgy Add方法如果传递了flowName字段，会根据flowName获取对应的流程id，直接打开受理页面

var sFlow = {
    v: '1.0.1',
    //受理
    Add: function (flowName) {
      
        if ($.isNull(flowName)) {
            $.modalOpen({
                id: "Form",
                title: "选择模板",
                url: "/WfClient/Designer/SelectFLow",
                width: "900px",
                height: "500px",
                maxmin: true,    //弹出即全屏
                callBack: function (iframeId) {
                    //选择模板的回调方法会传递回对应的模板id
                    top.frames[iframeId].submitForm(
                        function (processDefId) {
                            sFlow.CreateTask(processDefId);
                        });
                }
            });
        } else {
            $.get("/WfClient/Designer/SelectFLowId", { FlowName: flowName },
                function (processDefId) {
            
                    if (processDefId) {
                        sFlow.CreateTask(processDefId);
                    } else {
                        $.modalMsg(String.format("没有找到名称为{0}的流程模板", flowName), "error");
                    }
                });
        }
    },

    //创建任务
    CreateTask: function (processDefId) {
        $.ajax({
            type: 'POST',
            url: "/WfClient/TaskCenter/CreateTask",
            data: { processDefId: processDefId },
            dataType: "json",
            success: function (result) {
                //无页面值，直接提交流程
                if ($.isNull(result.data)) {
                    var grid = $("#gridList").data("kendoGrid");
                    grid.dataSource.read();
                    grid.refresh();
                    $.modalMsg(result.message, result.state);
                } else { //有data值，打开对应的页面
                    $.modalOpen({
                        id: "WfProcess",
                        title: result.message,
                        url: result.data,
                        width: "1024px",
                        height: "600px",
                        maxmin: true,   
                        fullScreen:true,
                        callBack: function (iframeId) {
                            top.frames[iframeId].submitFlow(
                                function () {
                                    $("#gridList").data('kendoGrid').dataSource.read();
                                    $("#gridList").data('kendoGrid').refresh();
                                }
                            );
                        }
                    });
                }
            }
        });
    },

    //接收任务
    Receive: function () {
        var grid = $("#gridList").data("kendoGrid");
        var row = grid.select();
        var data = grid.dataItem(row);
        debugger
        if (!$.isNull(data)) {
            $.ajax({
                type: 'POST',
                url: "/WfClient/TaskCenter/ReceiveTask",
                data: { WORKITEMID: data.WORKITEMID, PROCESSID: data.PROCESSID, VERSION: data.VERSION },
                dataType: "json",
                success: function (result) {
                    var grid = $("#gridList").data("kendoGrid");
                    grid.dataSource.read();
                    grid.refresh();
                    $.modalMsg(result.message, result.state);
                }
            });
        }
    },

    //审批任务
    Approve: function () {
        var grid = $("#gridList").data("kendoGrid");
        var row = grid.select();
        var data = grid.dataItem(row);
        if (!$.isNull(data)) {
            $.ajax({
                type: 'POST',
                url: "/WfClient/TaskCenter/CompleteTask",
                data: {
                    WORKITEMID: data.WORKITEMID
                },
                dataType: "json",
                success: function (result) {
                    //无页面值，直接提交流程
                    if ($.isNull(result.data)) {
                        var grid = $("#gridList").data("kendoGrid");
                        grid.dataSource.read();
                        grid.refresh();
                        $.modalMsg(result.message, result.state);
                    } else { //有data值，打开对应的页面
                        sFlow.OpenWfDialog(result.message, result.data + "&OperType=Approve");
                    }
                }
            });
        }
    },

    //打回
    Reject: function () {
        var grid = $("#gridList").data("kendoGrid");
        var row = grid.select();
        var data = grid.dataItem(row);
        if (!$.isNull(data)) {
            $.ajax({
                type: 'POST',
                url: "/WfClient/TaskCenter/RejectTask",
                data: {
                    WORKITEMID: data.WORKITEMID,
                },
                dataType: "json",
                success: function (result) {
                    //var grid = $("#gridList").data("kendoGrid");
                    //grid.dataSource.read();
                    //grid.refresh();
                    //$.modalMsg(result.message, result.state);
                    //无页面值，直接提交流程
                    if ($.isNull(result.data)) {
                        var grid = $("#gridList").data("kendoGrid");
                        grid.dataSource.read();
                        grid.refresh();
                        $.modalMsg(result.message, result.state);
                    } else { //有data值，打开对应的页面
                        sFlow.OpenWfDialog(result.message, result.data + "&OperType=Reject");
                    }
                }
            });
        }
    },

    OpenWfDialog: function (title, url) {
        $.modalWfProcessOpen({
            id: "WfProcess",
            title: title,
            url: url,
            width: "1024px",
            height: "600px",
            maxmin: true,    //弹出即全屏
            callBack: function (iframeId, index, oper) {
                debugger
                if (oper == 0) {
                    top.frames[iframeId].submitFlow(
                        function () {
                            $("#gridList").data('kendoGrid').dataSource.read();
                            $("#gridList").data('kendoGrid').refresh();
                        }
                    );
                } else if (oper == 1) {
                    top.frames[iframeId].rejectFlow(
                        function () {
                            $("#gridList").data('kendoGrid').dataSource.read();
                            $("#gridList").data('kendoGrid').refresh();
                        }
                    );
                }
            }
        });
    }
}