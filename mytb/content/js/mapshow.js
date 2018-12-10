//地图加载
var tempArr = $("#lnglat").val().split(",");
var map = new AMap.Map("mapcontainer", {
    resizeEnable: true,
    zoom: 12,
    center: [tempArr[0], tempArr[1]],
});
addMarker(tempArr[0], tempArr[1]); 
function addMarker(lng, lat) {
    map.clearMap();
    var marker = new AMap.Marker({
        map: map,
        position: [lng, lat]
    });
    AMap.event.addListener(marker, 'click', function () {
        openInfo(marker);
    });
}
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