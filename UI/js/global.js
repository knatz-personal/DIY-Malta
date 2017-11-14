var GLOBALSCRIPT = {
    Tooltips: function () {
        $(document).tooltip({
            hide: 'fade',
            position: {
                my: "right center",
                at: "left center",
                collision: "flipfit flipfit"
            },
            items: "select,input[title]"
        });
    },
    Dialogs: function () {
        $("#dialog-message").dialog({
            modal: true,
            autoOpen: false,
            title: "Message",
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
      
    },
    Datepickers: function () {
        $('.datepicker').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "yy-mm-dd",
            showAnim: "clip",
            yearRange: "-100:+15",
            autoSize: false,
            constrainInput: true
        });
    },
    CartSummary: function () {
        $.ajax({
            type: "POST",
            url: "/default.aspx/GetCartSummary",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: "json",
            data: JSON.stringify({}),
            success: function (data) {
                $("#cart-counter").text(JSON.parse(data.d).ItemCount);
                $("#cart-total").text(JSON.parse(data.d).CartTotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            }
        });
    },
    UXInit: function () {
        this.CartSummary();
        this.Tooltips();
        this.Dialogs();
        this.Datepickers();
    },
    Test: function () {
        console.log("GLOBALSCRIPT loaded successfully!");
    },
    Init: function () {
        this.Test();
        this.UXInit();
    }
};

function SetUpLoader() {
    (function ($) {
        $.widget("artistan.loading", $.ui.dialog, {
            options: {
                // your options
                spinnerClassSuffix: 'spinner',
                spinnerHtml: '',// allow for spans with callback for timeout...
                maxHeight: false,
                maxWidth: false,
                minHeight: 150,
                minWidth: 150,
                height: 150,
                width: 150,
                modal: true
            },

            _create: function () {
                $.ui.dialog.prototype._create.apply(this);
                // constructor
                $(this.uiDialog).children('*').hide();
                var self = this,
                options = self.options;
                self.uiDialogSpinner = $('.ui-dialog-content', self.uiDialog)
                    .html(options.spinnerHtml)
                    .addClass('ui-dialog-' + options.spinnerClassSuffix);
            },
            _setOption: function (key, value) {
                var original = value;
                $.ui.dialog.prototype._setOption.apply(this, arguments);
                // process the setting of options
                var self = this;

                switch (key) {
                    case "innerHeight":
                        // remove old class and add the new one.
                        self.uiDialogSpinner.height(value);
                        break;
                    case "spinnerClassSuffix":
                        // remove old class and add the new one.
                        self.uiDialogSpinner.removeClass('ui-dialog-' + original).addClass('ui-dialog-' + value);
                        break;
                    case "spinnerHtml":
                        // convert whatever was passed in to a string, for html() to not throw up
                        self.uiDialogSpinner.html("" + (value || '&#160;'));
                        break;
                }
            },
            _size: function () {
                $.ui.dialog.prototype._size.apply(this, arguments);
            },
            // other methods
            loadStart: function (newHtml) {
                if (typeof (newHtml) != 'undefined') {
                    this._setOption('spinnerHtml', newHtml);
                }
                this.open();
            },
            loadStop: function () {
                this._setOption('spinnerHtml', this.options.spinnerHtml);
                this.close();
            }
        });
    })(jQuery);
}

$(function() {
    GLOBALSCRIPT.Init();
    SetUpLoader();
});

