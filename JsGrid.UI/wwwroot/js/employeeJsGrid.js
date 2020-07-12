
/* 
 * Prior to ES6 - when defining a method for a object literal,
 * we must specify the name and full definiation
 */

//let empGrid = {
//    name: 'Employee Grid',
//    getdata: function () {
//        console.log('data retrive from service');
//    }
//}


/* ES6 syntax */
let empGrid = {
    name: 'Employee Grid',
    dataUrl: 'http://localhost:4911/service/getdata',
    filterDataUrl:'http://localhost:4911/service/getfilterdata',
    getData() {
        console.log(this.dataUrl);
        console.log(this.filterDataUrl);
    }
}


/* Constructor function*/
function Employee(gridId) {
    this.gridId = gridId;

};

Employee.prototype.Init = function() {
    const param = { tenantId: 150 };
    var $that = this;
    var promise = $that.getGridFiltersData(param);
    promise
        .then(function (result) {
            var filterCollection = {
                ddlState: [],
                ddlGender: []
            };
            if (result.data.stateList.length > 0) {
                filterCollection.ddlState.push({ text: "Select All", value: "0" });
                result.data.stateList.forEach(function(item) {
                    filterCollection.ddlState.push({ text: item, value: item });
                });
            }
            if (result.data.genderList.length > 0) {
                filterCollection.ddlGender.push({ text: "Select All", value: "0" });
                result.data.genderList.forEach(function (item) {
                    filterCollection.ddlGender.push({ text: item, value: item });
                });
            }

            $that.populateGrid(param,filterCollection);

        }).done(function(res, stat, xhr) {

    });


};

Employee.prototype.getGridFiltersData = function(param) {

    const filter = { tenantId: param.tenantId };

    const d = $.Deferred();
    $.ajax({
        type:"GET",
        url: "https://localhost:44356/api/employee/getFilterValues",
        dataType: "json",
        data: filter
    }).done(function (response) {
        const da = response;
        d.resolve(da);
    });
    return d.promise();
};

Employee.prototype.populateGrid = function (param,filterCollection) {
    $that = this;
    $("#jsGrid").jsGrid({
        height: "500px",
        width: "100%",
        heading: true,
        filtering: true,
        inserting: false,
        editing: false,
        selecting: true,
        sorting: true,
        paging: true,
        pageLoading: true,
        autoload: true,
        pagerContainer: null,
        pageIndex: 1,
        pageSize: $("#pageSize")[0].value,
        pageButtonCount: 5,
        updateOnResize: true,
        updateOnRowChange: true,
        pagerFormat: " {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount} {itemCount}",
        pagePrevText: "Prev",
        pageNextText: "Next",
        pageFirstText: "First",
        pageLastText: "Last",
        pageNavigatorNextText: "...",
        pageNavigatorPrevText: "...",
        invalidMessage: "Please review the error",
        controller: {
            loadData: this.getGridData
        },
        fields: [
            { type: "control", width: 50, editButton: false, deleteButton: false },
            { title: "Id", name: "id", type: "text", width: 300 },
            { title: "Salutation",name: "salutation", type: "text", width: 100 },
            { title: "First Name",name: "firstName", type: "text", width: 100 },
            { title: "MI",name: "middleName", type: "text", width: 100 },
            { title: "Last Name",name: "lastName", type: "text", width: 100 },
            {
                title:"Gender",
                name: "gender",
                type: "select",
                width: 150,
                items: filterCollection.ddlGender,
                valueField: "value",
                textField: "text",
                filterTemplate:function() {
                    var $select = jsGrid.fields.select.prototype.filterTemplate.call(this);
                    return $select;
                }

            },
            { title:"Email", name: "email", type: "text", width: 300 },
            { title: "Phone", name: "phone", type: "text", width: 120 },
            { title: "Address Line",name: "addressLine", type: "text", width: 250 },
            { title: "City", name: "city", type: "text", width: 200 },
            {
                title: "State",
                name: "state",
                type: "select",
                width: 150,
                items: filterCollection.ddlState,
                valueField: "value",
                textField: "text",
                filterTemplate: function () {
                    var $select = jsGrid.fields.select.prototype.filterTemplate.call(this);
                    return $select;
                }

            },
            { title: "ZipCode",name: "zipCode", type: "text", width: 150 },
            { title: "DOB", name: "dateOfBirth", type: "text", width: 150 },
            { title: "Salary", name: "salary", type: "text", width: 150, itemTemplate: function (value) { return $that.formatNumber(value); } },
            { title: "SSN",name: "ssn", type: "text", width: 100 }
        ],
        rowClick:function(args) {
            //Deselect all the row
            for (var i = 0; i < this.data.length; i++) {
                this.rowByIndex(i).removeClass("highlight");
            }

            let $row = this.rowByItem(args.item),
                selectedRow = $("#jsGrid").find('table tr.highlight');

            if (selectedRow.length) {
                selectedRow.toggleClass('highlight');
            }
            $row.toggleClass("highlight");
        }
    });
};

Employee.prototype.getGridData = function (filter) {
    //add extra param to filter if needed 
    filter.extraParam = "this is a extra param";

    let d = $.Deferred();
    $.ajax({
        url: "https://localhost:44356/api/employee/getEmployees",
        dataType: "json",
        data: filter
    }).done(function (response) {
        $("#totalRecords").text(response.totalCount);
        let da = { data: response.items, itemsCount: response.totalCount };
        d.resolve(da);
    });
    return d.promise();

};

Employee.prototype.formatNumber = function(numberToFormat) {
    let formatter = new Intl.NumberFormat('en-US',
        {
            style: 'currency',
            currency: 'USD',
            digits: 2
        });
    return formatter.format(numberToFormat);
};




