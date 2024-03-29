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
    l.dbId = 0, l.dbname = null, l.collist = [], l.isCheckAll = 0, $("#checkAll").click(function () {
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
            "Not exist" == o(e.columnId, l.collist) && l.collist.push({
                ColumnId: e.columnId,
                ColumnName: e.columnName,
                DataType: e.dataType,
                MaxLength: e.maxLength,
                IsNullable: e.isNullable,
                TableSchema: e.tableSchema,
                Tablename: e.tablename
            })
        } else {
            var n = l.collist.indexOf(e.ColumnId); - 1 < n && l.collist.splice(n, 1)
        }
    }, l.generate = function () {
        $('.nav-tabs a[href="#views"]').tab("show");
        var n = [],
            o = "genCodeSql";
        if (0 < l.collist.length) {
            var e = "[" + JSON.stringify(l.collist) + "]";
            t({
                method: "POST",
                url: "/api/Codegen/GenerateCode",
                data: e,
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).then(function (e) {
                if ($("#genCodeSql").text(""), $("#genCodeVm").text(""), $("#genCodeVu").text(""), $("#genCodeAngular").text(""), $("#genCodeAPI").text(""), 0 < (n = e.data.spCollection).length)
                    for (var t = 0; t < n.length; t++)
                        0 == t ? document.getElementById(o).innerHTML += "--+++++++++ SET SP +++++++ \r\n" + n[t] + "\r\n" :
                        1 == t ? document.getElementById(o).innerHTML += "--+++++++++ GET SP +++++++++ \r\n" + n[t] + "\r\n" :
                        2 == t ? document.getElementById(o).innerHTML += "--+++++++++ PUT SP +++++++++ \r\n" + n[t] + "\r\n" :
                        3 == t ? document.getElementById(o).innerHTML += "--+++++++++ DELETE SP +++++++++ \r\n" + n[t] + "\r\n" :
                        4 == t ? document.getElementById("genCodeVm").innerHTML += "// +++++++++ MODEL PROPERTIES +++++++++ \r\n" + n[t] + "\r\n" :
                        5 == t ? document.getElementById("genCodeVu").innerHTML += "\x3c!-- +++++++++ HTML FORM +++++++++ --\x3e \r\n" + n[t] + "\r\n" :
                        6 == t ? document.getElementById("genCodeAngular").innerHTML += "// +++++++++ AngularJS Controller +++++++++ \r\n" + n[t] + "\r\n" :
                        7 == t ? document.getElementById("genCodeAPI").innerHTML += "// +++++++++ API Controller +++++++++ \r\n" + n[t] + "\r\n" :
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
            if (t[o].columnId == e) {
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