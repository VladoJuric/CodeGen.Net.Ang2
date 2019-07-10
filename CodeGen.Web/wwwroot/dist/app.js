var templatingApp;
! function () {
    "use strict";
    templatingApp = angular.module("templating_app", ["ui.router"])
}(), templatingApp.config(["$locationProvider", "$stateProvider", "$urlRouterProvider", "$urlMatcherFactoryProvider", "$compileProvider", function (e, t, n, o, l) {
    window.history && window.history.pushState && e.html5Mode({
        enabled: !0,
        requireBase: !0
    }).hashPrefix("!"), o.strictMode(!1), l.debugInfoEnabled(!1), t.state("home", {
        url: "/",
        templateUrl: "./views/home/home.html",
        controller: "HomeController"
    }).state("about", {
        url: "/about",
        templateUrl: "./views/about/about.html",
        controller: "AboutController"
    }), n.otherwise("/home")
}]), templatingApp.controller("AboutController", ["$scope", "$http", function (e, t) {
    e.title = "About Page"
}]), templatingApp.controller("HomeController", ["$scope", "$http", function (l, t) {
    l.dbId = 0, l.dbname = null, l.collist = [], l.isCheckAll = 0, l.tblName = null, $("#checkAll").click(function () {
        $("input:checkbox").not(this).prop("checked", this.checked);
        var e = $('#checkboxes input:checked[name="coList[]"]').map(function () {
            return $(this).val()
        }).get().length;
        if (0 < e)
            for (var t = 1; t <= e; t++) {
                var n = document.getElementById("chkc_" + t);
                angular.element(n).triggerHandler("click")
            } else l.collist = []
    }), t({
        method: "GET",
        url: "/api/Codegen/GetDatabaseList"
    }).then(function (e) {
        l.dblist = e.data
    }, function (e) {
        console.log(e)
    }), l.getAllTable = function (e) {
        l.dbId = e.databaseId, l.dbname = e.databaseName, l.dbModel = {
            DatabaseId: e.databaseId,
            DatabaseName: e.databaseName
        }, t({
            method: "POST",
            url: "/api/Codegen/GetDatabaseTableList",
            data: l.dbModel
        }).then(function (e) {
            l.tblist = e.data
        }, function (e) {
            console.log(e)
        })
        }, l.getAllTableColumn = function (e) {
	    l.tblName = e.tableName;
        l.dbModel = {
            DatabaseId: l.dbId,
            DatabaseName: l.dbname,
            TableId: e.tableId,
            TableName: e.tableName
        }, t({
            method: "POST",
            url: "/api/Codegen/GetDatabaseTableColumnList",
            data: l.dbModel
        }).then(function (e) {
            l.colist = e.data
        }, function (e) {
            console.log(e)
        })
    }, l.getColumn = function (e, t) {
        if (t) {
            "Not exist" === o(e.columnId, l.collist) && l.collist.push({
                ColumnId: e.columnId,
                ColumnName: e.columnName,
                DataType: e.dataType,
                MaxLength: e.maxLength,
                IsNullable: e.isNullable,
                TableSchema: e.tableSchema,
                Tablename: e.tablename
            })
        } else {
            var n = l.collist.indexOf(e); - 1 < n && l.collist.splice(n, 1)
        }
    }, l.generate = function () {
        $('.nav-tabs a[href="#views"]').tab("show");
        var n = [],
            o = "genCodeSql";
			c = "genCodeVm";
        if (0 < l.collist.length) {
	        console.log("Columns: ", l.collist);
            var e = "{ " +
                "DatabaseName: " + JSON.stringify(l.dbname) + ", " +
                "TableName: " + JSON.stringify(l.tblName) + ", " +
                "Columns: " + JSON.stringify(l.collist) + " }";
            t({
                method: "POST",
                url: "/api/Codegen/GenerateCode",
                data: e,
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).then(function (e) {
                //l.dbname = null;
                //l.tblName = null;
                l.collist = [];
                if ($("#genCodeSql").text(""), $("#genCodeVm").text(""), $("#genCodeVu").text(""), $("#genCodeAPI").text(""), $("#genCodeRepoEF").text(""), $('#genCodeSI').text(""), 0 < (n = e.data.spCollection).length)
                    for (var t = 0; t < n.length; t++)
                        0 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL INSERT SP +++++++ \r\n" + n[t] + "\r\n" :
                        1 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL SELECT SP +++++++++ \r\n" + n[t] + "\r\n" :
                        2 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL SELECT BY ID SP +++++++++ \r\n" + n[t] + "\r\n" :
                        3 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL UPDATE SP +++++++++ \r\n" + n[t] + "\r\n" :
                        4 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL DELETE SP +++++++++ \r\n" + n[t] + "\r\n" :
                        5 === t ? document.getElementById(o).innerHTML += "--+++++++++ SQL VIEW SP +++++++++ \r\n" + n[t] + "\r\n" :
                        6 === t ? document.getElementById(c).innerHTML += "// +++++++++ CORE IENTITY PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        7 === t ? document.getElementById(c).innerHTML += "// +++++++++ CORE MODIFIEDENTITY AND DTO PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        8 === t ? document.getElementById(c).innerHTML += "// +++++++++ CORE ENTITIES PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        9 === t ? document.getElementById(c).innerHTML += "// +++++++++ CORE DTOS PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        10 === t ? document.getElementById(c).innerHTML += "// +++++++++ CORE INTERFACE PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        11 === t ? document.getElementById("genCodeVu").innerHTML += "\x3c!-- +++++++++ HTML FORM +++++++++ --\x3e \r\n" + n[t] + "\r\n" :
                        12 === t ? document.getElementById("genCodeAPI").innerHTML += "// +++++++++ API CONTROLLER +++++++++ \r\n" + n[t] + "\r\n" :
                        13 === t ? document.getElementById("genCodeSI").innerHTML += "// +++++++++ API INTERFACE +++++++++ \r\n" + n[t] + "\r\n" :
                        14 === t ? document.getElementById("genCodeSI").innerHTML += "// +++++++++ API SERVICE +++++++++ \r\n" + n[t] + "\r\n" :
                        15 === t ? document.getElementById("genCodeRepoEF").innerHTML += "// +++++++++ REPOSITORY DBCONTEXT PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        16 === t ? document.getElementById("genCodeRepoEF").innerHTML += "// +++++++++ REPOSITORY UNITOFWORK PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        17 === t ? document.getElementById("genCodeRepoEF").innerHTML += "// +++++++++ EF REPOSITORY CRUD PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        18 === t ? document.getElementById("genCodeRepoADO").innerHTML += "// +++++++++ ADO.NET REPOSITORY PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
						document.getElementById(o).innerHTML += " Error !!"
            }, function (e) {
                console.log(e)
            })
        } else n = [], $("#genCodeSql").text(""), $("#genCodeVm").text(""), console.log("Please Choose a Column!!")
    }, l.reset = function () {
        l.collist = [], rowGen = [], $("#genCodeSql").text(""), $("#genCodeVm").text("")
    };
    var o = function (e, t) {
        for (var n = "Not exist", o = 0; o < t.length; o++) {
            if (t[o].columnId === e) {
                n = "Exist";
                break
            }
        }
        return n
    }
}]), templatingApp.directive("topNavbarmenu", function () {
    return {
        restrict: "EA",
        templateUrl: "views/shared/navbar/nav.html"
    }
}), templatingApp.directive("fixedSidebarleft", function () {
    return {
        restrict: "EA",
        templateUrl: "views/shared/sidebar/menu.html"
    }
});