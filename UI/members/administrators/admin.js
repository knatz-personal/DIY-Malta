
function JTOptions(identifier, name, defaultSorting, fields) {
    $(identifier).jtable({
        title: 'The ' + name + ' List',
        paging: true,
        sorting: true,
        defaultSorting: defaultSorting,
        actions: {
            listAction: '/members/administrators/index.aspx/' + name + 'List',
            createAction: '/members/administrators/index.aspx/Create' + name,
            updateAction: '/members/administrators/index.aspx/Update' + name,
            deleteAction: '/members/administrators/index.aspx/Delete' + name
        },
        fields: fields,
        //Initialize validation logic when a form is created
        formCreated: function (event, data) {
            data.form.validationEngine();
        },
        //Validate form when it is being submitted
        formSubmitting: function (event, data) {
            return data.form.validationEngine('validate');
        },
        //Dispose validation logic when form is closed
        formClosed: function (event, data) {
            data.form.validationEngine('hide');
            data.form.validationEngine('detach');
        }
    });

    $(identifier).jtable('load');
}

function JTAdvancedOptions(identifier, title, defaultSorting, operations, fields, events) {
    $(identifier).jtable({
        title: title,
        paging: true,
        sorting: true,
        defaultSorting: defaultSorting,
        actions: operations,
        fields: fields,
        //Initialize validation logic when a form is created
        formCreated: function (event, data) {
            data.form.validationEngine();
        },
        //Validate form when it is being submitted
        formSubmitting: function (event, data) {
            return data.form.validationEngine('validate');
        },
        //Dispose validation logic when form is closed
        formClosed: function (event, data) {
            data.form.validationEngine('hide');
            data.form.validationEngine('detach');
        }
    });
    events();
}

var ADMINDASHBOARD = {
    Tabs: function () {
        $("#tabs").tabs();
    },
    Dropdowns: function () {
        function loadList(method, identifier) {
            $.ajax({
                type: "POST",
                url: "/members/administrators/index.aspx/" + method,
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (data) {
                    var result = data.d;
                    $.each(result.Options, function (key, value) {
                        $(identifier).append($("<option></option>").val
                        (value.Value).html(value.DisplayText));
                    });
                }
            });
        }

        loadList("GetOrderStateOptions", "#osoption");
    },
    loadNavigationTables: function () {
        $("#options5").selectmenu({
            style: 'dropdown',
            transferClasses: true,
            width: 300,
            change: function () {
                var clearTableContainer = function () {
                    $("#selectedTable5").empty();
                };
                var tableContainer = $("#selectedTable5");
                var optionSelected = $(this).find("option:selected");
                var valueSelected = optionSelected.val();

                if (valueSelected == -1) {
                    clearTableContainer();
                } else if (valueSelected == 1) {
                    var menu = {
                        //CHILD TABLE DEFINITION FOR "Roles"
                        Role: {
                            title: 'Role',
                            width: '0.01%',
                            paging: false,
                            sorting: false,
                            edit: false,
                            create: false,
                            display: function (menuData) {
                                //Create an image that will be used to open child table
                                var $img = $('<img src="/img/key.png" title="Manage Roles" />');
                                //Open child table when user clicks the image
                                $img.click(function () {
                                    $('#menusgrid').jtable('openChildTable',
                                        $img.closest('tr'),
                                        {
                                            title: menuData.record.Name + ' - Roles',
                                            actions: {
                                                listAction: '/members/administrators/index.aspx/GetMenuRoles?menuId=' + menuData.record.ID,
                                                createAction: '/members/administrators/index.aspx/CreateMenuRole'
                                            },
                                            fields: {
                                                MenuID: {
                                                    type: 'hidden',
                                                    defaultValue: menuData.record.ID
                                                },
                                                RoleID: {
                                                    title: 'Name',
                                                    list: true,
                                                    options: '/members/administrators/index.aspx/GetRoleOptions'
                                                },
                                                RemoveMenuRole: {
                                                    title: '',
                                                    width: '0.01%',
                                                    sorting: false,
                                                    create: false,
                                                    edit: false,
                                                    list: true,
                                                    display: function (data) {
                                                        var $removeBttn = $('<div style="margin:0 auto; width:16px;" ><img src="/img/delete.png" title="Delete Menu Role" /></div>');
                                                        $removeBttn.click(function () {
                                                            $.ajax({
                                                                type: "POST",
                                                                url: "index.aspx/DeleteMenuRole",
                                                                data: JSON.stringify({
                                                                    menuId: data.record.MenuID,
                                                                    roleId: data.record.RoleID
                                                                }),
                                                                contentType: "application/json; charset=utf-8",
                                                                dataType: "json",
                                                                success: function () {
                                                                    $removeBttn.closest('tr').remove();
                                                                }
                                                            });
                                                        });
                                                        return $removeBttn;
                                                    }
                                                }
                                            }
                                        }, function (data) { //opened handler
                                            data.childTable.jtable('load');
                                        });
                                });
                                //Return image to show on the parent row
                                return $img;
                            }
                        },
                        ID: {
                            key: true,
                            title: "ID",
                            create: false,
                            edit: false,
                            list: false
                        },
                        Name: {
                            title: 'Name',
                            width: '2%',
                            inputClass: 'validate[required]'
                        },
                        RootParent: {
                            title: 'Main Parent',
                            options: '/members/administrators/index.aspx/GetRootMenuOptions',
                            list: false
                        },
                        ParentID:
                        {
                            title: 'Parent',
                            width: '2%',
                            dependsOn: 'RootParent',
                            options: function (data) {
                                if (data.source == 'list') {
                                    return '/members/administrators/index.aspx/GetSubMenuOptions?parentID=0';
                                }
                                return '/members/administrators/index.aspx/GetSubMenuOptions?parentID=' + data.dependedValues.RootParent;
                            }
                        },
                        Description: {
                            title: 'Description',
                            type: 'textarea',
                            inputClass: 'validate[required]'
                        },
                        Url: {
                            title: 'Url',
                            list: false,
                            inputClass: 'validate[required]'
                        }
                    };
                    clearTableContainer();
                    tableContainer.append("<div id=\"menusgrid\"></div>");

                    $('#menusgrid').jtable({
                        title: 'The Menu List',
                        paging: true,
                        sorting: true,
                        defaultSorting: 'ID ASC',
                        actions: {
                            listAction: '/members/administrators/index.aspx/MenuList',
                            createAction: '/members/administrators/index.aspx/CreateMenu',
                            updateAction: '/members/administrators/index.aspx/UpdateMenu',
                            deleteAction: '/members/administrators/index.aspx/DeleteMenu'
                        },
                        fields: menu,
                        //Initialize validation logic when a form is created
                        formCreated: function (event, data) {
                            data.form.validationEngine();
                            var root = $("#Edit-RootParent");
                            var parent = $("#Edit-ParentID");
                            parent.parent().parent().hide();
                            root.change(function () {
                                var value = root.val();
                                if (value === -1) {
                                    parent.parent().parent().hide();
                                } else if ($('#Edit-ParentID option').length > 0) {
                                    parent.parent().parent().show();
                                    parent.prepend("<option value='-1' selected='selected'>None</option>");
                                } else if ($('#Edit-ParentID option').length === 0) {
                                    parent.parent().parent().hide();
                                }
                            });

                            root.prepend("<option value='-1' selected='selected'>None</option>");
                            data.form.find('select option:first').attr("selected", "selected");
                        },
                        //Validate form when it is being submitted
                        formSubmitting: function (event, data) {
                            return data.form.validationEngine('validate');
                        },
                        //Dispose validation logic when form is closed
                        formClosed: function (event, data) {
                            data.form.validationEngine('hide');
                            data.form.validationEngine('detach');
                        }
                    });
                    $("#menusgrid").jtable('load');
                } else if (valueSelected == 2) {
                    var category = {
                        ID: {
                            key: true,
                            title: "ID",
                            create: false,
                            edit: false,
                            list: false
                        },
                        Name: {
                            title: 'Name',
                            inputClass: 'validate[required]'
                        },
                        RootParent: {
                            title: 'Main Parent',
                            list: false,
                            options: '/members/administrators/index.aspx/GetRootCategoryOptions'
                        },
                        ParentID:
                        {
                            title: 'Parent',
                            list: true,
                            dependsOn: 'RootParent',
                            options: function (data) {
                                if (data.source == 'list') {
                                    return '/members/administrators/index.aspx/GetSubCategoryOptions?parentID=0';
                                }
                                return '/members/administrators/index.aspx/GetSubCategoryOptions?parentID=' + data.dependedValues.RootParent;
                            }
                        },
                        Description: {
                            title: 'Description',
                            type: 'textarea',
                            inputClass: 'validate[required]'
                        }
                    };
                    clearTableContainer();
                    tableContainer.append("<div id=\"categorysgrid\"></div>");
                    $('#categorysgrid').jtable({
                        title: 'The Category List',
                        paging: true,
                        sorting: true,
                        defaultSorting: 'ID ASC',
                        actions: {
                            listAction: '/members/administrators/index.aspx/CategoryList',
                            createAction: '/members/administrators/index.aspx/CreateCategory',
                            updateAction: '/members/administrators/index.aspx/UpdateCategory',
                            deleteAction: '/members/administrators/index.aspx/DeleteCategory'
                        },
                        fields: category,
                        //Initialize validation logic when a form is created
                        formCreated: function (event, data) {
                            data.form.validationEngine();
                            var root = $("#Edit-RootParent");
                            var parent = $("#Edit-ParentID");
                            parent.parent().parent().hide();
                            root.change(function () {
                                var value = root.val();
                                if (value === -1) {
                                    parent.parent().parent().hide();
                                } else if ($('#Edit-ParentID option').length > 0) {
                                    parent.parent().parent().show();
                                    parent.prepend("<option value='-1' selected='selected'>None</option>");
                                } else if ($('#Edit-ParentID option').length === 0) {
                                    parent.parent().parent().hide();
                                }
                            });

                            root.prepend("<option value='-1' selected='selected'>None</option>");
                            data.form.find('select option:first').attr("selected", "selected");
                        },
                        //Validate form when it is being submitted
                        formSubmitting: function (event, data) {
                            return data.form.validationEngine('validate');
                        },
                        //Dispose validation logic when form is closed
                        formClosed: function (event, data) {
                            data.form.validationEngine('hide');
                            data.form.validationEngine('detach');
                        }
                    });
                    $("#categorysgrid").jtable('load');
                }
            }
        }).selectmenu("menuWidget").addClass("overflow");
    },
    LoadSettingTables: function () {
        $("#options6").selectmenu({
            style: 'dropdown',
            transferClasses: true,
            width: 300,
            change: function () {
                var basic = {
                    ID: {
                        key: true,
                        list: false,
                        create: false,
                        edit: false,
                        title: "ID"
                    },
                    Name: {
                        title: 'Name',
                        inputClass: 'validate[required]'
                    }
                };

                var clearTableContainer = function () {
                    $("#selectedTable6").empty();
                };
                var tableContainer = $("#selectedTable6");
                var optionSelected = $(this).find("option:selected");
                var valueSelected = optionSelected.val();

                if (valueSelected == -1) {
                    clearTableContainer();
                } else if (valueSelected == 1) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"rolesgrid\"></div>");
                    JTOptions('#rolesgrid', 'Role', 'Name ASC', basic);
                    $("#rolesgrid").jtable('load');
                } else if (valueSelected == 2) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"townsgrid\"></div>");
                    JTOptions('#townsgrid', 'Town', 'Name ASC', basic);
                    $("#townsgrid").jtable('load');
                } else if (valueSelected == 3) {
                    clearTableContainer();
                    tableContainer.append(" <div id=\"gendersgrid\"></div>");
                    JTOptions('#gendersgrid', 'Gender', 'Name ASC', basic);
                    $("#gendersgrid").jtable('load');
                } else if (valueSelected == 4) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"usertypesgrid\"></div>");
                    JTOptions('#usertypesgrid', 'UserType', 'Name ASC', basic);
                    $("#usertypesgrid").jtable('load');
                } else if (valueSelected == 5) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"orderstatesgrid\"></div>");
                    JTOptions('#orderstatesgrid', 'OrderState', 'Name ASC', basic);
                    $("#orderstatesgrid").jtable('load');
                } else if (valueSelected == 6) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"contacttypesgrid\"></div>");
                    JTOptions('#contacttypesgrid', 'ContactType', 'Name ASC', basic);
                    $("#contacttypesgrid").jtable('load');
                } else if (valueSelected == 7) {
                    clearTableContainer();
                    tableContainer.append("<div id=\"addresstypesgrid\"></div>");
                    JTOptions('#addresstypesgrid', 'AddressType', 'Name ASC', basic);
                    $("#addresstypesgrid").jtable('load');
                }
            }
        }).selectmenu("menuWidget").addClass("overflow");
    },
    LoadAccountTables: function () {
        var userf = {
            //CHILD TABLE DEFINITION FOR "Roles"
            Role: {
                title: 'Role',
                width: '2%',
                paging: false,
                sorting: false,
                edit: false,
                create: false,
                display: function (roleData) {
                    //Create an image that will be used to open child table
                    var $img = $('<img src="/img/key.png" title="Manage Roles" />');
                    //Open child table when user clicks the image
                    $img.click(function () {
                        $('#usersgrid').jtable('openChildTable',
                            $img.closest('tr'),
                            {
                                title: roleData.record.Username + ' - Roles',
                                actions: {
                                    listAction: '/members/administrators/index.aspx/GetRoles?Username=' + roleData.record.Username,
                                    createAction: '/members/administrators/index.aspx/AllocateRole'
                                },
                                fields: {
                                    Username: {
                                        type: 'hidden',
                                        defaultValue: roleData.record.Username
                                    },
                                    RoleID: {
                                        title: 'Name',
                                        list: true,
                                        options: '/members/administrators/index.aspx/GetRoleOptions',
                                        inputClass: 'validate[required]'
                                    },
                                    RemoveRole: {
                                        title: '',
                                        width: '.08%',
                                        sorting: false,
                                        create: false,
                                        edit: false,
                                        list: true,
                                        display: function (data) {
                                            var $img1 = $('<img src="/img/delete.png" title="Deallocate role" />');
                                            $img1.click(function () {
                                                $.ajax({
                                                    type: "POST",
                                                    url: "index.aspx/DeAllocateRole",
                                                    data: JSON.stringify({
                                                        Username: data.record.Username,
                                                        RoleID: data.record.RoleID
                                                    }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function () {
                                                        $img1.closest('tr').remove();
                                                    }
                                                });
                                            });
                                            return $img1;
                                        }
                                    }
                                }
                            }, function (data) { //opened handler
                                data.childTable.jtable('load');
                            });
                    });
                    //Return image to show on the parent row
                    return $img;
                }
            },
            //CHILD TABLE DEFINITION FOR "Contacts"
            Contact: {
                title: 'Contact',
                width: '2%',
                paging: false,
                sorting: false,
                edit: false,
                create: false,
                display: function (contactData) {
                    //Create an image that will be used to open child table
                    var $img = $('<img src="/img/phone_metro.png" title="Edit contact details" />');
                    //Open child table when user clicks the image
                    $img.click(function () {
                        $('#usersgrid').jtable('openChildTable',
                            $img.closest('tr'),
                            {
                                title: contactData.record.FirstName + ' ' + contactData.record.LastName + ' - Contact Details',
                                actions: {
                                    listAction: '/members/administrators/index.aspx/ContactList?Username=' + contactData.record.Username
                                    //,updateAction: '/members/administrators/index.aspx/',
                                },
                                fields: {
                                    Username: {
                                        type: 'hidden',
                                        defaultValue: contactData.record.Username
                                    },
                                    ID: {
                                        key: true,
                                        create: false,
                                        edit: false,
                                        list: false
                                    },
                                    Email: {
                                        title: 'Email',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    Phone: {
                                        title: 'Phone Number',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    Mobile: {
                                        title: 'Mobile Number',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    TypeID: {
                                        title: 'Contact Type',
                                        list: true,
                                        options: '/members/administrators/index.aspx/GetContactTypeOptions'
                                    }
                                }
                            }, function (data) { //opened handler
                                data.childTable.jtable('load');
                            });
                    });
                    //Return image to show on the parent row
                    return $img;
                }
            },
            //CHILD TABLE DEFINITION FOR "Address"
            Address: {
                title: 'Address',
                width: '2%',
                sorting: false,
                edit: false,
                create: false,
                display: function (addressData) {
                    //Create an image that will be used to open child table
                    var $img = $('<img src="/img/list_metro.png" title="Edit Address Detail" />');
                    //Open child table when user clicks the image
                    $img.click(function () {
                        $('#usersgrid').jtable('openChildTable',
                            $img.closest('tr'), //Parent row
                            {
                                title: addressData.record.FirstName + ' ' + addressData.record.LastName + ' - Address Details',
                                actions: {
                                    listAction: '/members/administrators/index.aspx/AddressList?Username=' + addressData.record.Username
                                },
                                fields: {
                                    Username: {
                                        type: 'hidden',
                                        defaultValue: addressData.record.Username
                                    },
                                    ID: {
                                        key: true,
                                        create: false,
                                        edit: false,
                                        list: false
                                    },
                                    Residence: {
                                        title: 'Residence',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    Street: {
                                        title: 'Street',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    TownID: {
                                        title: 'Town',
                                        list: true,
                                        options: '/members/administrators/index.aspx/GetTownOptions'
                                    },
                                    PostCode: {
                                        title: 'Post Code',
                                        list: true,
                                        inputClass: 'validate[required]'
                                    },
                                    TypeID: {
                                        title: 'Address Type',
                                        list: true,
                                        options: '/members/administrators/index.aspx/GetAddressTypeOptions'
                                    }
                                }
                            }, function (data) { //opened handler
                                data.childTable.jtable('load');
                            });
                    });
                    //Return image to show on the person row
                    return $img;
                }
            },
            Username: {
                title: 'Username',
                key: true,
                create: false,
                edit: false
            },
            FirstName: {
                title: 'Name',
                inputClass: 'validate[required]'
            },
            MiddleInitial: {
                title: 'Initials'
            },
            LastName: {
                title: 'Surname',
                inputClass: 'validate[required]'
            },
            DateOfBirth: {
                title: 'Birth Date',
                type: 'date',
                displayFormat: 'yy-mm-dd',
                inputClass: 'validate[required,custom[date]]'
            },
            GenderID: {
                title: 'Gender',
                options: '/members/administrators/index.aspx/GetGenders'
            },
            Password: {
                title: 'Password',
                type: 'password',
                list: false,
                inputClass: 'validate[required]'
            },
            Blocked: {
                title: 'Status',
                type: 'checkbox',
                values: { 'false': 'UnBlocked', 'true': 'Blocked' },
                defaultValue: 'false'
            },
            NoOfAttempts: {
                title: 'Failed Logins',
                list: false
            },
            UserType: {
                title: 'User Type',
                options: '/members/administrators/index.aspx/GetUserTypes'
            }
        };

        var actions = {
            listAction: '/members/administrators/index.aspx/ListUserByFilter',
            updateAction: '/members/administrators/index.aspx/UpdateUser',
            deleteAction: '/members/administrators/index.aspx/DeleteUser'
        };

        JTAdvancedOptions('#usersgrid', 'The User Account List', 'Username ASC',
            actions,
            userf,
            function () {
                //Re-load records when user click 'load records' button.
                $('#LoadUserRecords').click(function (e) {
                    e.preventDefault();
                    $('#usersgrid').jtable('load', {
                        username: $('#username1').val()
                    });
                });

                //Load all records when page is first shown
                $('#LoadUserRecords').click();
            });
    },
    LoadSalesTables: function () {
        var salef = {
            ID: {
                title: 'ID',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            Name: {
                title: 'Name',
                inputClass: 'validate[required]'
            },
            DateStarted: {
                title: 'Start ',
                width: '0.5%',
                type: "date",
                inputClass: 'validate[required,custom[date]]'
            },
            DateExpired: {
                title: 'End ',
                width: '0.5%',
                type: "date",
                inputClass: 'validate[required,custom[date]]'
            },
            Discount: {
                title: 'Discount %',
                width: '1%',
                inputClass: 'validate[required,custom[number]]'
            }
        };

        var actions = {
            listAction: '/members/administrators/index.aspx/ListSaleByFilter',
            createAction: '/members/administrators/index.aspx/CreateSale',
            updateAction: '/members/administrators/index.aspx/UpdateSale',
            deleteAction: '/members/administrators/index.aspx/DeleteSale'
        };

        JTAdvancedOptions('#salesgrid', 'The Promotional Sales List', 'DateStarted DESC',
            actions,
            salef,
            function () {
                //Re-load records when user click 'load records' button.
                $('#LoadProductSalesRecords').click(function (e) {
                    e.preventDefault();
                    $('#salesgrid').jtable('load', {
                        query: $('#query2').val(),
                        start: $('#sDate').val(),
                        end: $('#eDate').val()
                    });
                });

                //Load all records when page is first shown
                $('#LoadProductSalesRecords').click();
            }
        );
    },
    LoadOrderTables: function () {
        var stateId = 0;
        $("#osoption").val("0");
        $("#osoption").selectmenu({
            style: 'dropdown',
            transferClasses: true,
            width: 312,
            change: function () {
                var optionSelected = $(this).find("option:selected");
                stateId = optionSelected.val();
            }
        }).selectmenu("menuWidget").addClass("overflow");

        var orderf = {
            ID: {
                title: 'ID',
                key: true,
                list: false,
                create: false,
                edit: false
            },
            Print: {
                title: '',
                sorting: false,
                edit: false,
                create: false,
                width: '1%',
                list: true,
                listClass: "jtable-command-column",
                display: function (orderData) {

                    var $img = $('<a href="/members/invoice.aspx?orderId=' + orderData.record.ID + '">' +
                        '<img src="/img/print.png" title="Print Invoice" alt="Print Invoice" /></a>');
                    return $img;
                }
            },
            Username: {
                title: 'Username',
                edit: false,
                create: false
            },
            DatePlaced: {
                title: 'Date Placed',
                type: 'date',
                edit: false
            },
            State: {
                title: 'Status',
                options: '/members/administrators/index.aspx/GetOrderStateOptions',
                inputClass: 'validate[required]'
            },
            //CHILD TABLE DEFINITION FOR "Order Details"
            Detail: {
                title: '',
                width: '0.1%',
                paging: false,
                sorting: false,
                edit: false,
                create: false,
                list: true,
                listClass: "jtable-command-column",
                display: function (orderData) {
                    var $img = $('<button class="jtable-command-button" title="Manage Order Details"><img src="/img/list_metro.png" title="Manage Order Details" style="width:16px;height:16px;" /></button>');

                    row = $img.closest('tr');

                    $img.click(function (event) {
                        $('#ordersgrid').jtable('openChildTable',
                            $img.closest('tr'),
                            {
                                title: 'Details of Order',
                                actions: {
                                    listAction: '/members/administrators/index.aspx/GetOrderDetails?id=' + orderData.record.ID,
                                    updateAction: '/members/administrators/index.aspx/UpdateOrderDetail'
                                },
                                fields: {
                                    OrderID: {
                                        type: 'hidden',
                                        key: true,
                                        defaultValue: orderData.record.ID
                                    },
                                    ProductID: {
                                        title: 'ProductID',
                                        type: 'hidden'

                                    },
                                    Username: {
                                        title: 'Username',
                                        type: 'hidden'
                                    },
                                    Item: {
                                        title: 'Item',
                                        edit: false
                                    },
                                    UnitPrice: {
                                        title: 'UnitPrice',
                                        list: true,
                                        edit: false
                                    },
                                    Quantity: {
                                        title: 'Quantity'
                                    },
                                    Stock: {
                                        title: 'Available Stock',
                                        edit:false
                    },
                                    TotalPrice: {
                                        title: 'TotalPrice',
                                        edit: false
                                    },
                                    Info: {
                                        title: '',
                                        sorting: false,
                                        edit: false,
                                        create: false,
                                        width: '1%',
                                        list: true,
                                        listClass: "jtable-command-column",
                                        display: function (data) {

                                            var $img = $('<a href="/detail.aspx?productId=' + data.record.ProductID + '">' +
                                                '<img title="Product Detail" src="/img/icn_alert_info.png" alt="Product Detail" /></a>');
                                            return $img;
                                        }
                                    },
                                    RemoveItem: {
                                        title: '',
                                        width: '1%',
                                        sorting: false,
                                        create: false,
                                        edit: false,
                                        list: true,
                                        listClass: "jtable-command-column",
                                        display: function (data) {
                                            var $removeBttn = $('<button class="jtable-command-button" title="Remove Item"><img src="/img/delete.png" alt="Remove Item" ></button>');
                                            $removeBttn.click(function (event3) {
                                                $.ajax({
                                                    type: "POST",
                                                    url: "index.aspx/DeleteOrderDetail",
                                                    data: JSON.stringify({
                                                        OrderID: data.record.OrderID,
                                                        ProductID: data.record.ProductID,
                                                        Username: data.record.Username
                                                    }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function () {
                                                        $removeBttn.closest('tr').remove();
                                                    }
                                                });
                                                event3.preventDefault();
                                            });
                                            return $removeBttn;
                                        }
                                    }
                                },
                                //Initialize validation logic when a form is created
                                formCreated: function (event2, data) {
                                    data.form.validationEngine();
                                },
                                //Validate form when it is being submitted
                                formSubmitting: function (event2, data) {

                                    return data.form.validationEngine('validate');
                                },
                                //Dispose validation logic when form is closed
                                formClosed: function (event2, data) {
                                    data.form.validationEngine('hide');
                                    data.form.validationEngine('detach');
                                }
                            }, function (data) { //opened handler
                                data.childTable.jtable('load');
                            });

                        event.preventDefault();
                    });

                    return $img;
                }
            }
        };

        var actions = {
            listAction: '/members/administrators/index.aspx/ListOrderByFiter',
            updateAction: '/members/administrators/index.aspx/UpdateOrder',
            deleteAction: '/members/administrators/index.aspx/DeleteOrder'
        };

        JTAdvancedOptions('#ordersgrid', 'The Orders List', 'DatePlaced DESC',
            actions,
            orderf,
            function () {

                query = "";

                ADMINDASHBOARD.FindUsername("#osearch");

                //Re-load records when user click 'load records' button.
                $('#LoadOrderRecords').click(function (e) {
                    e.preventDefault();
                    $('#ordersgrid').jtable('load', {
                        query: $('#osearch').val(),
                        state: stateId,
                        start: $('#osDate').val(),
                        end: $('#oeDate').val()
                    });
                });

                //Load all records when page is first shown
                $('#LoadOrderRecords').click();
            }
        );
    },
    FindUsername: function (identifier) {
        $(identifier).val("");
        $(identifier).autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: "/members/administrators/index.aspx/FindUsername",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ query: request.term }),
                    success: function (data) {
                        response(data.d);
                    }
                });
            },
            select: function (event, ui) {
                query = ui.item.value;
            },
            open: function (event, ui) {
                $(".ui-autocomplete").css("z-index", 1000);
            }
        });
    },

    //TODO: Finish image display
    LoadCatalogTables: function () {
        var prodf = {
            ID: {
                key: true,
                title: "ID",
                create: false,
                edit: false,
                list: false
            },
            Image: {
                title: 'Image',
                type: 'file',
                create: true,
                edit: true,
                list: true,
                width: '.1%',
                display: function (data) {
                    var imageUrl = data.record.Image;
                    recName = data.record.Name;
                    return '<img src="' + data.record.Image + '" width="150px" height="150px" alt="' + data.record.Name + '"  >';
                },
                input: function (data) {
                    var imageUrl;
                    var recName;
                    if (typeof imageUrl === "undefined" && typeof recName === "undefined") {
                        imageUrl = "/img/catalogue/product0.jpg";
                        recName = "Product Image";
                    }
                    return '<form id="uploadForm" action="/uploader.aspx" method="post" enctype="multipart/form-data" target="uploadTrigger">' +
                        '<div id="fileUploader" style="text-align:left;"><input type="file" name="file" id="fileUpload">' +
                        '<button id="submitBttn" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" type="submit" role="button"> <span class="ui-button-text">Upload</span></button></div>' +
                        '</form>' +
                        '<iframe id="uploadTrigger" name="uploadTrigger" height="0" width="0" frameborder="0"scrolling="no" style="display:none"></iframe>' +
                        '<div style="text-align:left;padding:5px;clear:both;"><img id="prodImg" style="border-width:0px;" src="' + imageUrl + '" width="300px" height="300px" alt="' + recName + '"   /></div>';
                }
            },
            Name: {
                title: 'Name',
                inputClass: 'validate[required]'
            },
            Description: {
                title: 'Description',
                type: 'textarea',
                list: false,
                inputClass: 'validate[required]'
            },
            CategoryID: {
                title: 'Category',
                options: '/members/administrators/index.aspx/GetCategoryOptions',
                inputClass: 'validate[required]',
                width: '1%'
            },
            Stock: {
                title: 'Stock',
                inputClass: 'validate[required]',
                width: '1%'
            },
            VAT: {
                title: 'Tax %',
                inputClass: 'validate[required]',
                width: '1%'
            },
            IsActive: {
                title: 'Status',
                width: '1%',
                type: 'checkbox',
                values: { 'false': 'Disabled', 'true': 'Active' },
                defaultValue: 'true'
            },
            SaleID: {
                title: 'Sale',
                width: '1%',
                inputClass: 'validate[required]',
                options: '/members/administrators/index.aspx/GetSalesOptions'
            },
            //CHILD TABLE DEFINITION FOR "Price Types"
            Price: {
                title: 'Price',
                width: '0.1%',
                paging: false,
                sorting: false,
                edit: false,
                create: false,
                listClass: "jtable-command-column",
                display: function (priceData) {
                    //Create an image that will be used to open child table
                    var $pricebttn = $('<button class="jtable-command-button" title="Manage Prices"><img src="/img/money.png" title="Manage Prices" style="width:24px;height:24px;" /></button>');
                    //Open child table when user clicks the image
                    $pricebttn.click(function (event) {
                        $('#productsgrid').jtable('openChildTable',
                            $pricebttn.closest('tr'),
                            {
                                title: priceData.record.Name + ' - Price Types',
                                actions: {
                                    listAction: '/members/administrators/index.aspx/GetPriceTypes?ProductID=' + priceData.record.ID,
                                    createAction: '/members/administrators/index.aspx/CreatePriceType',
                                    updateAction: '/members/administrators/index.aspx/UpdatePriceType'
                                },
                                fields: {
                                    ProductID: {
                                        type: 'hidden',
                                        defaultValue: priceData.record.ID
                                    },
                                    UserType: {
                                        key: true,
                                        create: true,
                                        edit: false,
                                        title: 'User Type',
                                        options: '/members/administrators/index.aspx/GetUserTypes'
                                    },
                                    UnitPrice: {
                                        title: 'Unit Price',
                                        inputClass: 'validate[required]'
                                    },
                                    RemovePriceType: {
                                        title: '',
                                        width: '1%',
                                        sorting: false,
                                        create: false,
                                        edit: false,
                                        list: true,
                                        display: function (data) {
                                            var $removeBttn = $('<div style="margin:0 auto; width:16px;" ><img src="/img/delete.png" title="Delete Price Type" /></div>');
                                            $removeBttn.click(function () {
                                                $.ajax({
                                                    type: "POST",
                                                    url: "index.aspx/DeletePriceType",
                                                    data: JSON.stringify({
                                                        ProductID: data.record.ProductID,
                                                        UserType: data.record.UserType
                                                    }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function () {
                                                        $removeBttn.closest('tr').remove();
                                                    }
                                                });
                                            });
                                            return $removeBttn;
                                        }
                                    }
                                },
                                //Initialize validation logic when a form is created
                                formCreated: function (event2, data) {
                                    data.form.validationEngine();
                                },
                                //Validate form when it is being submitted
                                formSubmitting: function (event2, data) {
                                    return data.form.validationEngine('validate');
                                },
                                //Dispose validation logic when form is closed
                                formClosed: function (event2, data) {
                                    data.form.validationEngine('hide');
                                    data.form.validationEngine('detach');
                                }
                            }, function (data) { //opened handler
                                data.childTable.jtable('load');
                            });
                        event.preventDefault();
                    });
                    //Return image to show on the parent row
                    return $pricebttn;
                }
            }
        };

        $('#productsgrid').jtable({
            title: 'The Product List',
            paging: true,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: '/members/administrators/index.aspx/ListProductByFilter',
                createAction: '/members/administrators/index.aspx/CreateProduct',
                updateAction: '/members/administrators/index.aspx/UpdateProduct',
                deleteAction: '/members/administrators/index.aspx/DeleteProduct'
            },
            fields: prodf,
            //Initialize validation logic when a form is created
            formCreated: function (event, data) {
                data.form.validationEngine();
            },
            //Validate form when it is being submitted
            formSubmitting: function (event, data) {
                return data.form.validationEngine('validate');
            },
            //Dispose validation logic when form is closed
            formClosed: function (event, data) {
                data.form.validationEngine('hide');
                data.form.validationEngine('detach');
            }
        });

        //Re-load records when user click 'load records' button.
        $('#LoadProductRecords').click(function (e) {
            e.preventDefault();
            $('#productsgrid').jtable('load', {
                query: $('#query1').val()
            });
        });

        //Load all records when page is first shown
        $('#LoadProductRecords').click();
    },

    Test: function () {
        console.log("ADMINDASHBOARD loaded successfully");
    },
    Init: function () {
        $("select").val("-1");
        this.Test();
        this.Tabs();
        this.Dropdowns();
        this.LoadSettingTables();
        this.loadNavigationTables();
        this.LoadAccountTables();
        this.LoadSalesTables();
        this.LoadOrderTables();
        this.LoadCatalogTables();
    }
};

$.ready(ADMINDASHBOARD.Init());