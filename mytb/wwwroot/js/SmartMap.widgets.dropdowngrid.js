//add by sgy 2017-08-28
(function ($) {
    var kendo = window.kendo,
        ui = kendo.ui,
        List = ui.List,
        Select = ui.Select,
        support = kendo.support,
        DropDownList = ui.DropDownList,
        data = kendo.data,
        DataSource = data.DataSource,
        Grid = ui.Grid,
        activeElement = kendo._activeElement,
        ObservableObject = kendo.data.ObservableObject,
        keys = kendo.keys,
        assert = window.assert,
        UNDEFINED = 'undefined',
        CHANGE = 'change',
        DIV = '<div/>',
        ns = '.kendoDropDownList',
        DISABLED = 'disabled',
        READONLY = 'readonly',
        CHANGE = 'change',
        FOCUSED = 'k-state-focused',
        DEFAULT = 'k-state-default',
        STATEDISABLED = 'k-state-disabled',
        ARIA_DISABLED = 'aria-disabled',
        HOVEREVENTS = 'mouseenter' + ns + ' mouseleave' + ns,
        TABINDEX = 'tabindex',
        STATE_FILTER = 'filter',
        STATE_ACCEPT = 'accept',
        MSG_INVALID_OPTION_LABEL = 'The `optionLabel` option is not valid due to missing fields. Define a custom optionLabel as shown here http://docs.telerik.com/kendo-ui/api/javascript/ui/dropdownlist#configuration-optionLabel',
        proxy = $.proxy;


    /*********************************************************************************
     * Helpers
     *********************************************************************************/

    ///**
    // * Processes a table (dropDownList dataSource) with dataParentField into a hierarchy (Hierarchical dataSOurce for treeView)
    // * Note: see also app.cache.getCategoryHierarchy
    // * @param dataSource
    // * @param idField
    // * @param dataParentField
    // * @returns {*}
    // */
    //function hierarchize(dataSource, idField, dataParentField) {
    //    var hash = {};
    //    for (var i = 0, total = dataSource.total(); i < total; i++) {
    //        var item = dataSource.at(i);
    //        var id = item.get(idField);
    //        var parentId = ($.isFunction(item[dataParentField]) ? item[dataParentField]() : item.get(dataParentField)) || 'root';
    //        hash[id] = hash[id] || [];
    //        hash[parentId] = hash[parentId] || [];
    //        // We need a bare item that can be converted into kendo.data.Node in a kendo.data.HierarchyDataSource
    //        item = item.toJSON();
    //        item.items = hash[id];
    //        hash[parentId].push(item);
    //    }
    //    return hash.root || [];
    //}

    /*********************************************************************************
     * Widget
     * 
     *********************************************************************************/

    var DropDownGrid = DropDownList.extend({

        /**
         * Initialization
         * @param element
         * @param options
         */
        init: function (element, options) {
            var that = this;
            options.dataSource = options.grid.dataSource;
            DropDownList.fn.init.call(that, element, options);
            that._layout();
            that.refresh();
        },
        
        /**
         * Options
         */
        options: {
            name: 'DropDownGrid',
        },

        /**
         * Events
         */
        /*
        events: [
            CHANGE,
            DATABINDING,
            DATABOUND
        ],
        */


        /**
         * Value
         * @param value
         */
        value: function (value) {
            var that = this;
            if ($.type(value) === UNDEFINED) {
                return DropDownList.fn.value.call(this);
            } else {
                var options = that.options;                
                debugger
                DropDownList.fn.value.call(this, value);               
                var dataItem = that.dataSource.data().find(function (record) { return record[options.dataValueField] === value; });
                if (dataItem === UNDEFINED)
                    dataItem = that._grid.dataSource.data().find(function (record) { return record[options.dataValueField] === value; });
                if (dataItem) {
                    that.text(dataItem[options.dataTextField]);
                    that.trigger(CHANGE);
                } else {
                    that.text('');
                }
            }
        },
        /**
        * dataItem
        * 获取当前选中的dataItem
        */
        dataItem: function () {
            var that = this;
            var options = that.options;
            var value = DropDownList.fn.value.call(this);
            debugger
            if ($.type(value) === UNDEFINED) {
                return null;
            } else {
                //var dataItem = that.dataSource.data().find(function (record) { return record[options.dataValueField] === value; });
                var dataItem = that._grid.dataSource.data().find(function (record) { return record[options.dataValueField] === value; });
                return dataItem;
            }
        },
        /**
         * Replace popup list with treeview
         * @private
         */
        _layout: function () {
            var that = this;
            var options = this.options;

            //assert.instanceof($, that.list, kendo.format(assert.messages.instanceof.default, 'this.list', 'jQuery'));

            // Find the popup list
            var popupList = $('ul', that.list);
            if (popupList.length) {

                // Destroy it
                var popupListWidget = kendo.widgetInstance(popupList);
                if (popupListWidget && $.isFunction(popupListWidget.destroy)) {
                    popupListWidget.destroy();
                }
                this.popup.element.css('width', parseInt(options.gridWidth) + 2);


                // Replace it with a div
                var popupGrid = $(DIV)
                    .css({ overflowX: 'scroll', overflowY: 'scroll', width: options.gridWidth });
             
                popupList.replaceWith(popupGrid);

                // Create the treeView
                that._grid = popupGrid.kendoGrid(options.grid).data("kendoGrid");

                that._grid.bind("filterMenuInit", function (e) {
                    that.popup.canClose = false;            
                    var wrapper = e.container.find("input");
                    e.container.addClass(DEFAULT).removeClass(STATEDISABLED).on(HOVEREVENTS, that._toggleHover);

                    var btns = e.container.find("button");
                    //绑定click获取焦点
                    wrapper.on("click", function () {
                        this.focus();
                    });
              
                    btns.on("click", function () {
                        this.focus();
                    });
                });
           
                that._grid.bind("change", function (e) {
                    //debugger
                    // e.preventDefault();
                   
                    var selectedRows = this.select();
                    var dataItem = this.dataItem(selectedRows[0]);
                    //that.text(dataItem[options.dataTextField]);
                    that.value(dataItem[options.dataValueField]);
                    that.trigger(CHANGE);
                    that.popup.canClose = true;
                    that.popup.close();
                });
                that._grid.bind("page", function (e) {
                    // prevent closing popup when collapsing a node
                    that.popup.canClose = false;
                    var grid = that._grid;
                    var selectedNode = "tr[" + options.dataKeyField + "='" + options.dataValueField + "']";
                    grid.select(selectedNode);
                    that.setDataSource(that._grid.dataSource);
                });
                that._grid.bind("filter", function (e) {
                  
                    that.popup.canClose = false;
              
                    var grid = that._grid;
                    var selectedNode = "tr[" + options.dataKeyField + "='" + options.dataValueField + "']";
                    grid.select(selectedNode);
                    that.setDataSource(that._grid.dataSource);
                });

                //debugger
                this.options.height = 100;

                // Keep flag for avoiding to close the popup
                // when expanding and collapsing nodes
                that.popup.canClose = true;

                // Replace _closeHandler
                that._closeHandler = function (e) {
                    that.popup.canClose = true;
                    kendo.ui.DropDownList.fn._closeHandler.call(this, e);
                };

                // Bind treeview mousedown event
                // This captures the mousedown on the scroller that closes the popup in IE and Edge
                // If an element is selected, the select event of te treeview allows the popup to close
                // @see https://github.com/jlchereau/Kidoju-Webapp/issues/170
                popupGrid.on('mousedown', function (e) {
                    that.popup.canClose = false;
                });

                // Bind popup open and close events
                that.popup
                    .bind('open', function () {
                        if (that.value()) {
                            var grid = that._grid;
                            //var selectedNode = grid.findByText(that.text());
                            //treeview.expandTo(selectedNode);
                            var selectedNode = "tr[" + options.dataKeyField + "='" + options.dataValueField + "']";
                            grid.select(selectedNode);
                        }
                    })
                    .bind('close', function (e) {
                        if (!that.popup.canClose) {
                            e.preventDefault();
                            that.popup.canClose = true;
                        }
                    });
            }
        },

        /**
         *
         * @returns {*}
         */
        items: function () {
            // return DropDownList.fn.items.call(this);
            return this._grid.items();
        },

        /**
         * Replace dataSource
         * @param dataSource
         */
        setDataSource: function (dataSource) {
            //// TODO
            DropDownList.fn.setDataSource.call(this, dataSource);
        },

        /**
         * Initialize dataSource
         * @param dataSource
         */
        _dataSource: function () {
            var that = this;
            DropDownList.fn._dataSource.call(that);
            // bind to the change event to refresh the widget
            if (that.dataSource instanceof DataSource) {
                if (that._refreshHandler) {
                    that.dataSource.unbind(CHANGE, that._refreshHandler);
                }
                that._refreshHandler = $.proxy(that.refresh, that);
                that.dataSource.bind(CHANGE, that._refreshHandler);
            }
        },

        /**
         * Refresh
         */
        refresh: function (e) {
            var that = this;
            var options = that.options;
            DropDownList.fn.refresh.call(that, e);
            //if (this._grid instanceof Grid) {
            //    //this._grid.setDataSource(hierarchize(that.dataSource, options.dataValueField, options.dataParentField));
            //    this._grid.setDataSource(that.dataSource);
            //    this._grid.dataSource.read();
            //    this._grid.refresh();
            //}
            that.value(options.value);
        },

        /**
         * Destroy
         */
        destroy: function () {
            var that = this;
            // TODO: unbind dataSource
            this._grid.destroy();
            DropDownList.fn.destroy.call(this);
        }
    });

    kendo.ui.plugin(DropDownGrid);
})(jQuery);