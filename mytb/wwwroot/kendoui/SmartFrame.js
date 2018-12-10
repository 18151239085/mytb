$.reload = function () {
    location.reload();
    return false;
}
var loadingIdx;
$.loading = function (bool, text) {
    if (bool) {
        if (!!text) {
            loadingIdx= top.layer.msg(text, {
                icon: 16
                , shade: 0.01
            });
        } else {
            loadingIdx= top.layer.msg('加载中', {
                icon: 16
                , shade: 0.01
            });
        }
        //top.layer.load();
    } else {
        top.layer.closeAll('loading');
        top.layer.close(loadingIdx);
        //layer.closeAll('msg');
    }
}

$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return decodeURI(ar[1]);
            }
        }
    }
    return "";
}


$.currentWindow = function () {
    //debugger
    var iframeId = top.$(".jqadmin-iframe:visible").attr("data-id");
    return top.frames[iframeId];
}

$.fn.formValid = function () {
    //return $(this).valid({
    //    debug: true,
    //    onError: function (error, inputElement) {
    //        alert(error);
    //    },
    //    errorPlacement: function (error, element) {
    //        debugger
    //        element.parents('.formValue').addClass('has-error');
    //        element.parents('.has-error').find('i.error').remove();
    //        element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '"></i>');
    //        $("[data-toggle='tooltip']").tooltip();
    //        if (element.parents('.input-group').hasClass('input-group')) {
    //            element.parents('.has-error').find('i.error').css('right', '33px')
    //        }
    //    },
    //    success: function (element) {
    //        element.parents('.has-error').find('i.error').remove();
    //        element.parent().removeClass('has-error');
    //    }
    //});
    //modify 2017-09-13使用kendoValidator来进行前端验证
    var errorTemplate = '<div class="k-widget k-tooltip k-tooltip-validation"' +
        'style="margin:0.5em;width:auto;"><span class="k-icon k-i-information"> </span>' +
        '#=message#<div class="k-callout k-callout-n"></div></div>'

    var validator = $(this).kendoValidator({errorTemplate: errorTemplate}).data("kendoValidator");
    //validatable.bind("validateInput", function (e) {
    //    console.log("input " + e.input.attr("name") + " changed to valid: " + e.valid);
    //})
    if (validator.validate()) {
        return true;
    } else {       
        $(".k-tooltip-validation").parent().parent(".k-widget").removeClass("k-valid");
        $(".k-tooltip-validation").parent().parent(".k-widget").addClass("k-invalid");
        $(".k-tooltip-validation").parent(".k-widget").removeClass("k-valid");
        $(".k-tooltip-validation").parent(".k-widget").addClass("k-invalid");
        return false;
    }
}


//$.fn.formSerialize = function (formdate) {
//    var element = $(this);
//    if (!!formdate) {
//        for (var key in formdate) {
//            var $id = element.find('#' + key);
//            var value = $.trim(formdate[key]).replace(/ /g, '');
//            var type = $id.attr('type');
//            if ($id.hasClass("select2-hidden-accessible")) {
//                type = "select";
//            }
//            switch (type) {
//                case "checkbox":
//                    if (value == "true") {
//                        $id.attr("checked", 'checked');
//                    } else {
//                        $id.removeAttr("checked");
//                    }
//                    break;
//                case "select":
//                    $id.val(value).trigger("change");
//                    break;
//                default:
//                    $id.val(value);
//                    break;
//            }
//        };
//        return false;
//    }
//    var postdata = {};
//    element.find('input,select,textarea').each(function (r) {
//        var $this = $(this);
//        var id = $this.attr('id');
//        var type = $this.attr('type');
//        switch (type) {
//            case "checkbox":
//                postdata[id] = $this.is(":checked");
//                break;
//            default:
//                var value = $this.val() == "" ? " " : $this.val();
//                if (!$.request("keyValue")) {
//                    value = value.replace(/ /g, '');
//                }
//                postdata[id] = value;
//                break;
//        }
//    });
//    if ($('[name=__RequestVerificationToken]').length > 0) {
//        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
//    }
//    return postdata;
//};

//弹出窗体，无按钮
$.modalDialog = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
    };
    var options = $.extend(defaults, options);
    var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    var index =top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: options.url,
        maxmin: true, //开启最大化最小化按钮       
    });
    if (options.fullScreen)
        top.layer.full(index);  //默认全屏
}
//debugger
$.modalOpen = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        btn: ['<span class="k-icon k-i-check-circle"></span> 确认', '<span class="k-icon k-i-close-circle"></span> 关闭'],
        btnclass: ['btn k-primary', 'btn k-danger'],
        callBack: null,
		endCallBack:null
    };
    var options = $.extend(defaults, options);

    var index = top.layer.open({
        id: options.id,
        type: 2 //此处以iframe举例
        , title: options.title
        , area: [options.width, options.height]
        , shade: options.shade
        , maxmin: true
        , content: options.url
        , btn: options.btn
        , yes: function (index, layero) {         
            options.callBack(layero.find('iframe')[0]['name'], index);
        }
        , cancel: function () {
            return true;
        },
        success: function (layero, index) {
            //debugger
            //console.log(layero, index);
			
        },end: function(){ //此处用于演示
			if (typeof(options.endCallBack)=="function"){
				options.endCallBack();
			}
			 
		}
    });
    if (options.fullScreen)
        top.layer.full(index);  //默认全屏
}

//流程的提交弹窗界面 add by sgy 2017-08-10
$.modalWfProcessOpen = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        //btn: ['<span class="k-icon k-i-check-circle"></span> 提交', '<span class="k-icon k-i-cancel-circle"></span> 打回', '<span class="k-icon k-i-close-circle"></span> 关闭'],
        //callBack: null
    };
    var options = $.extend(defaults, options);

    var index = top.layer.open({
        id: options.id,
        type: 2 //此处以iframe举例
        , title: options.title
        , area: [options.width, options.height]
        , shade: options.shade
        , maxmin: true
        , content: options.url
        //, btn: options.btn
        //, btn1: function (index, layero) {
        //    options.callBack(layero.find('iframe')[0]['name'], index, 0);
        //}, btn2: function (index, layero) {
        //    options.callBack(layero.find('iframe')[0]['name'], index, 1);
        //}, btn3: function (index, layero) {
        //    return true;
        //}
        ,cancel: function () {
            return true;
        }
    });
    top.layer.full(index);  //默认全屏
}

$.modalConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "系统提示",
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
    }, function (index) {
        //console.log(index);
        callBack(true, index);
    }, function (index) {
        callBack(false, index)
    });
}
$.modalAlert = function (content, type) {
    var icon = "";
    if (type == 'success') {
        icon = "fa-check-circle";
    }
    if (type == 'error') {
        icon = "fa-times-circle";
    }
    if (type == 'warning') {
        icon = "fa-exclamation-circle";
    }
    top.layer.alert(content, {
        icon: icon,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
    });
}
$.modalMsg = function (content, type) {
    if (type != undefined) {
        var icon = "";
        if (type == 'success') {
            icon = "fa-check-circle";
        }
        if (type == 'error') {
            icon = "fa-times-circle";
        }
        if (type == 'warning') {
            icon = "fa-exclamation-circle";
        }
        top.layer.msg(content, { icon: icon, time: 4000, shift: 5 });
        top.$(".layui-layer-msg").find('i.' + icon).parents('.layui-layer-msg').addClass('layui-layer-msg-' + type);
    } else {
        top.layer.msg(content);
    }
}
$.modalClose = function () {
    var index = top.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
    var IsClose = $IsdialogClose.is(":checked");
    if ($IsdialogClose.length == 0) {
        IsClose = true;
    }
    if (IsClose) {
        top.layer.close(index);
    } else {
        location.reload();
    }
}
$.submitForm = function (options) {
    var defaults = {
        url: "",
        param: [],
        loading: "正在提交数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    //$.loading(true, "准备提交数据");
    //debugger
    //setTimeOut偶尔会失效，注释掉
    //window.setTimeout(function () {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.state == "success") {
                    options.success(data);
                    $.modalMsg(data.message, data.state);
                    if (options.close == true) {
                        $.modalClose();
                    }
                } else {
                    $.modalAlert(data.message, data.state);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            beforeSend: function () {
                $.loading(true, options.loading);
            },
            complete: function () {
                $.loading(false);
            }
        });
    //}, 500);
}
$.deleteForm = function (options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "",
        param: [],
        loading: "正在删除数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $.loading(true, options.loading);
            window.setTimeout(function () {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data.state == "success") {
                            options.success(data);
                            $.modalMsg(data.message, data.state);
                        } else {
                            $.modalAlert(data.message, data.state);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false);
                        $.modalMsg(errorThrown, "error");
                    },
                    beforeSend: function () {
                        $.loading(true, options.loading);
                    },
                    complete: function () {
                        $.loading(false);
                    }
                });
            }, 500);
        }
    });

}

/** 
* 判断是否null
* ===比较类型也比较值
* @param data 
*/
$.isNull = function (data) {
    if (data === 0)
        return false;
    return (data == "" || data == undefined || data == null) ? "true" : false;
}


///sgy 封装的grid，用于统一grid的参数及操作
$.fn.SmartGrid = function (options) {

    var defaults = { selectable: "row" };

    var options = $.extend(defaults, options);
    var $element = $(this);
    //选中更改事件
    if ($.isNull(options["change"])) {
        options["change"] = function (rowid) {

            var length = $(this).select().length;
            var $operate = $(".operate");
            if (length > 0) {
                $operate.animate({ "left": 0 }, 200);
                if (typeof authorizeButton != 'undefined' && authorizeButton instanceof Function)
                    authorizeButton($(this).select()[0]);
            } else {
                $operate.animate({ "left": '-100.1%' }, 200);
            }
            $operate.find('.close').click(function () {
                $operate.animate({ "left": '-100.1%' }, 200);
            })
        };
    }
    if (options["height"] == undefined)
        options["height"] = getDocumentPort().height - 80;           //设置grid的高度

    options["resizable"] = true;                                 //设置grid允许更改列宽

    if (options["dataSource"].pageSize == undefined)
        options["dataSource"].pageSize = 30;


    options["dataSource"].serverPaging = true;
    options["dataSource"].serverFiltering = true;
    options["dataSource"].serverSorting = true;

    $element.kendoGrid(options);
};

$.fn.SmartTreeList= function (options) {

    var defaults = { selectable: "row" };

    var options = $.extend(defaults, options);
    var $element = $(this);
    if ($.isNull(options["change"])) {
        //选中更改事件
        options["change"] = function (rowid) {

            var length = $(this).select().length;
            var $operate = $(".operate");
            if (length > 0) {
                $operate.animate({ "left": 0 }, 200);
                if (typeof authorizeButton != 'undefined' && authorizeButton instanceof Function)
                    authorizeButton($(this).select()[0]);
            } else {
                $operate.animate({ "left": '-100.1%' }, 200);
            }
            $operate.find('.close').click(function () {
                $operate.animate({ "left": '-100.1%' }, 200);
            })
        };
    }
    if (options["height"] == undefined)
        options["height"] = getDocumentPort().height - 80;           //设置grid的高度

    options["resizable"] = true;                                 //设置grid允许更改列宽

    //if (options["dataSource"].pageSize == undefined)
    //    options["dataSource"].pageSize = 30;


    //options["dataSource"].serverPaging = true;
    //options["dataSource"].serverFiltering = true;
    //options["dataSource"].serverSorting = true;

    $element.kendoTreeList(options);
};

///注册grid的删除提示框，需将   editable: { confirmation: false} 否则会弹出系统的dialog
// 示例：regGridDeleteModal($("#gridList"));
function regGridDeleteModal(target) {
    // register delete click handler
    target.delegate(".k-grid-delete", "click", function (e) {
        e.preventDefault();
        var gridDelete = target.data("kendoGrid");
        var gridDeleteTarget = this;
        var tr = $(e.target).closest("tr"); //get the row for deletion
        var data = gridDelete.dataItem(tr);
        var datasource = gridDelete.dataSource;
        // display the modal
        $.modalConfirm('<h4 class="center">你确认要删除本条记录?</h4>', function (bool, index) {
            if (bool) {
                datasource.remove(data);
                datasource.sync();
                top.layer.close(index);
            } else {
                return true;
            }
        })
        return false;
    });

    //target.delegate(".k-grid-update", "click", function (e) {
    //    var grid = target.data("kendoGrid");
    //    grid.dataSource.read();
    //    grid.refresh();
    //});
}

/*视口的大小，部分移动设备浏览器对innerWidth的兼容性不好，需要
 *document.documentElement.clientWidth或者document.body.clientWidth
 *来兼容（混杂模式下对document.documentElement.clientWidth不支持）。
 *使用方法 ： getViewPort().width;
 */
function getViewPort() {
    if (document.compatMode == "BackCompat") {   //浏览器嗅探，混杂模式
        return {
            width: document.body.clientWidth,
            height: document.body.clientHeight
        };
    } else {
        return {
            width: document.documentElement.clientWidth,
            height: document.documentElement.clientHeight
        };
    }
}

//获得文档的大小（区别与视口）,与上面获取视口大小的方法如出一辙
function getDocumentPort() {
    if (document.compatMode == "BackCompat") {
        return {
            width: document.body.scrollWidth,
            height: document.body.scrollHeight
        };
    } else {
        return {
            width: Math.max(document.documentElement.scrollWidth, document.documentElement.clientWidth),
            height: Math.max(document.documentElement.scrollHeight, document.documentElement.clientHeight)
        }
    }
}

//将viewdata的结果转换成标准的json
function KendoDict(dict) {
    if (dict != null) {
        for (var i = 0, len = dict.length; i < len; i++) {
            var o = dict[i];
            for (var key in o) {
                o[key.toLowerCase().replace(/"(\w+)":/g, "$1:")] = o[key];
                delete (o[key]);
            }
        }
        return dict;
    } else {
        return "";

    }
}

//javascript 格式化方法
//var a = "I Love {0}, and You Love {1},Where are {0}! {4}";
//示例：String.format(a, "You","Me")
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}


//javascript 格式化方法
//示例：a.format("You","Me")
String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
} 


//验证工具栏的按钮
$.fn.authorizeButton = function () {
    var moduleId = top.$(".jqadmin-iframe:visible").attr("data-keyid");
    console.log(moduleId);
    var dataJson = top.clients.authorizedButtons[moduleId];
    //var dataJson = top.clients.authorizeButton[moduleId];
    var $element = $(this);
    $element.find('a[authorize=yes]').attr('authorize', 'no');
    $element.find('button[authorize=yes]').attr('authorize', 'no');

    if (dataJson != undefined) {
        $.each(dataJson, function (i) {
            $element.find("#" + dataJson[i].F_ENCODE).attr('authorize', 'yes');
        });
    }
    $element.find("[authorize=no]").parents('li').prev('.split').remove();
    $element.find("[authorize=no]").parents('li').remove();
    $element.find('[authorize=no]').remove();
}


//================================二进制字符串权限信息的判断Start=========================================
function IsBinPower(n)
{
    return (n & (n - 1)) == 0;
}

function GetBinPower(x)
{
    return Math.Pow(2, x);
}

//判断是否有权限,authCode 鉴权码,auth 权限值（2的幂级）
function BinIsHasAuth(authCode, auth)
{
    if (!IsBinPower(auth)) {
        console.log(String.Format("值 {0} 为无效的鉴权码不是2的幂级", auth));
        return;
    }

    if (authCode <= 0 || auth <= 0)
        return false;
    return auth == (authCode & auth);
}

//移除权限
function BinRemoveAuth(authCode, auth)
{
    if (!IsBinPower(auth)) {
        console.log(String.Format("值 {0} 为无效的鉴权码不是2的幂级", auth));
        return;
    }

    if (auth < 0 || auth > 4611686018427387904) {
        console.log(string.Format("鉴权值 {0} 应大于 0 小于 4611686018427387904", auth));
        return;
    }

    return code = authCode & (~auth);
}

function BinAddAuth(authCode, auth) {
    if (!IsBinPower(auth)) {
        console.log(String.Format("值 {0} 为无效的鉴权码不是2的幂级", auth));
        return;
    }

    if (auth < 0 || auth > 4611686018427387904)
    { 
        console.log(String.Format("鉴权值 {0} 应大于 0 小于 4611686018427387904", auth));
        return;
    }
    return code = authCode | auth;
}

function BinGenAuthCode(arr)
{
    if ($.isNull(arr)) {
        console.log(String.Format("权限值数组不允许为空,BinAuth.GenAuthCode()"));
        return;
    }
    var code = 0;
    for (var i = 0; i < arr.length; i++) {
        if (!IsBinPower(auth)) {
            console.log(String.Format("值 {0} 为无效的鉴权码不是2的幂级", auth));
            return;
        }

        if (auth < 0 || auth > 4611686018427387904) {
            console.log(String.Format("鉴权值 {0} 应大于 0 小于 4611686018427387904", auth));
            return;
        }
        code = code | arr[i];
    }
    return code;
}
//================================二进制字符串权限信息的判断End=========================================


//======================================公共函数==================================
function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
//================================================================================
