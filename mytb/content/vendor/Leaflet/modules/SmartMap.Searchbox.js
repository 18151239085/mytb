
function getControlHrmlContent() {

    var controlHtmlContent = "<div id=\"controlbox\" ><div id=\"boxcontainer\" class=\"searchbox searchbox-shadow\" ><div><input id=\"searchboxinput\" type=\"text\"  style=\"position: relative;\" /></div><div class=\"searchbox-searchbutton-container\"><button aria-label=\"search\"  id=\"searchbox-searchbutton\"  class=\"searchbox-searchbutton\"></button><span aria-hidden=\"true\"  style=\"display:none;\">search</span></div></div><div id=\"searchbox-searchResult\"  class=\"searchbox-searchResult searchbox-shadow\" hidden>\</div></div>	";
    return controlHtmlContent;
}


L.Control.SearchBox = L.Control.extend({
    _sideBarMenuItems: {
        _searchfunctionCallBack: function (x) {
            alert('calling the default search call back');
        }
    },
    options: {
        position: 'topleft'
    },
    initialize: function (options) {
        L.Util.setOptions(this, options);
    },
    onAdd: function (map) {
        var container = L.DomUtil.create('div');
        container.id = "controlcontainer";
        //var headerTitle = this._sideBarHeaderTitle;
        //var menuItems = this._sideBarMenuItems;
        var searchCallBack = this._searchfunctionCallBack;
        $(container).html(getControlHrmlContent());
        setTimeout(function () {

            $("#searchbox-searchbutton").click(function () {
                var searchkeywords = $("#searchboxinput").val();
                var html = searchCallBack(searchkeywords);
                if (html)
                {
                    $('#searchbox-searchResult').html(html);
                    $('#searchbox-searchResult').toggle(); 
                }
            });
        }, 1);


        L.DomEvent.disableClickPropagation(container);
        return container;
    }

});

//return searchboxControl;
//}

/**
 * Creates a new SearchBox.
 *
 */
L.control.SearchBox = function (options) {
    return new L.Control.SearchBox(options);
};
