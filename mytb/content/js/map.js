$(function () {
    //localstorage缓存获取
    var ls_dlwz = u.getStorage("MH_DLWZ");
    if (ls_dlwz) {
        $("#lnglat").val(ls_dlwz);
    }

    if ($("#lnglat").val()) {
        //已有坐标的地图加载
        var tempArr = $("#lnglat").val().split(",");
        var map = new AMap.Map("container", {
            resizeEnable: true,
            zoom: 12,
            center: [tempArr[0], tempArr[1]],
        });
        addMarker(map, tempArr[0], tempArr[1]);

        //为地图注册click事件获取鼠标点击出的经纬度坐标
        var clickEventListener = map.on('click', function (e) {
            document.getElementById("lnglat").value = e.lnglat.getLng() + ',' + e.lnglat.getLat()
            //标记
            addMarker(map,e.lnglat.getLng(), e.lnglat.getLat());

        })
        
    } else {
        //默认地图加载
        var map = new AMap.Map("container", {
            resizeEnable: true,
            zoom: 12,
            center: [106.35, 38.98],
        });

        //查询
            //输入提示
            var autoOptions = {
                input: "tipinput"
            };
            var auto = new AMap.Autocomplete(autoOptions);
            //构造地点查询类
            var placeSearch = new AMap.PlaceSearch({
                map: map,
            });
            //注册监听，当选中某条记录时会触发
            AMap.event.addListener(auto, "select", select);

        //关键字查询查询
        function select(e) {
            placeSearch.setCity(e.poi.adcode);
            placeSearch.search(e.poi.name);
        }

        //为地图注册click事件获取鼠标点击出的经纬度坐标
        var clickEventListener = map.on('click', function (e) {
            document.getElementById("lnglat").value = e.lnglat.getLng() + ',' + e.lnglat.getLat()
            //添加marker标记
            addMarker(map,e.lnglat.getLng(), e.lnglat.getLat());

        })
    
    }

})

//添加marker标记
function addMarker(map,lng, lat) {
    map.clearMap();
    var marker = new AMap.Marker({
        map: map,
        position: [lng, lat]
    });
     //鼠标点击marker弹出自定义的信息窗体
    AMap.event.addListener(marker, 'click', function () {
        openInfo(marker);
    });
}

var infoWindow;
//在指定位置打开信息窗体
function openInfo(marker) {
    var info = [];
    info.push("<div><div><img style=\"float:left;\" src=\" http://webapi.amap.com/images/autonavi.png \"/></div> ");
    info.push("地址 : 宁夏石嘴山市");
    info.push("邮编 : 753000 </div></div>");

    infoWindow = new AMap.InfoWindow({
        content: info.join("<br/>")  //使用默认信息窗体框样式，显示信息内容
    });
    infoWindow.open(map, marker.getPosition());
}