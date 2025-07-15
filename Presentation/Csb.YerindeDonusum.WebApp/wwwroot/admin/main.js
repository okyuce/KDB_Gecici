const ERROR_MESSAGES = {
    EMPTY_VALUE: "'{0}' alanı boş olamaz.",
    EMPTY_MULTIPLE: "Lütfen '{0}' ve '{1}' alanlarından en az birini doldurunuz.",
    INVALID_VALUE: "'{0}' alanı hatalı veya geçersiz.",
    EQUAL_LENGTH: "'{0}' alanı '{1}' karakter olmalı.",
    MIN_LENGTH: "'{0}' alanı en az '{1}' karakter olmalı.",
    MAX_LENGTH: "'{0}' alanı en fazla '{1}' karakter olmalı.",
    MIN_VALUE: "'{0}' alanı en az '{1}' olabilir.",
    MAX_VALUE: "'{0}' alanı en fazla '{1}' olabilir.",
    PLEASE_SELECT: "Lütfen '{0}' seçiniz.",
    MAX_FILE_COUNT: "'{0}' alanına en fazla '{1}' adet dosya yükleyebilirsiniz.",
    MAX_FILE_SIZE: "'{0}' dosyalarının toplam boyutu en fazla {1} Mb olabilir."
};

const CONSTANTS = {
    DEFAULT_MAX_FILE_COUNT: 5,
    DEFAULT_MAX_FILE_SIZE: 10, // mb
};

$(function () {
    moment.locale("tr");
    renderSelect2();
    confirmFunction();
    renderTable();
    renderTooltip();
    renderMask();
    onEnterPressed();
});

function getErrorMessage(errorMessage, ...args) {
    if (args?.length > 0) {
        for (let i = 0; i < args.length; i++) {
            errorMessage = errorMessage.replaceAll("{" + i + "}", args[i])
        }
        return errorMessage;
    }
    return "";
}
function alertError(errorMessage, ...args) {
    if (args?.length > 0) {
        for (let i = 0; i < args.length; i++) {
            errorMessage = errorMessage.replaceAll("{" + i + "}", args[i])
        }
        toastr.error(errorMessage);
    }
}

function onEnterPressed() {
    $("*[data-onenter-submit='true']").each(function () {
        let $this = $(this);
        $this.keyup(function (e) {
            if (e.keyCode == 13) {
                const isDisabled = $this.prop('disabled') ?? false;
                const isReadOnly = $this.prop('readonly') ?? false;
                if (isReadOnly || isDisabled) return;

                const functionOnEnter = $this.attr("data-onenter-function");
                const clickOnEnter = $this.attr("data-onenter-click");
                if (functionOnEnter) {
                    let param1 = $this.attr("data-param");
                    let param2 = $this.attr("data-param2");
                    if (param1 != undefined && param2 != undefined) {
                        window[functionOnEnter](param1, param2);
                    }
                    else if (param1 != undefined) {
                        window[functionOnEnter](param1);
                    }
                    else {
                        window[functionOnEnter]();
                    }
                }
                else if (clickOnEnter) {
                    $(clickOnEnter).click();
                }
            }
        });
    });
}

function renderMask() {
    $(".integer-mask").inputmask({
        alias: "integer",
        inputtype: "text",
        rightAlign: false
    });
    $(".double-mask").inputmask({
        alias: "decimal",
        radixPoint: window.culture?.numberDecimalSeparator || ",",
        inputtype: "text",
        rightAlign: false
    });
    $(".phone-mask").inputmask("0999 999 9999");
    $(".identity-mask").inputmask("99999999999");
    $(".email-mask").inputmask({ alias: "email" });
    $(".vergino-mask").inputmask("9999999999");
    $(".mersisno-mask").inputmask("9999999999999999");
    $(".iban-mask").inputmask("TR99-9999-9999-9999-9999-9999-99");
}

function getQueryStringValue(key) {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    return params[key] ?? false;
}

function validateEmail(email) {
    if (!(email?.length > 0)) return false;

    return String(email).toLowerCase().match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
}
function validateTCKN(tckn) {
    try {
        tckn = tckn.replace(/\D/g, '');
        tckn = tckn?.trim();
        // 11 haneden oluşmalıdır
        if (tckn?.length != 11) return false;
        // 0 ile başlayamaz
        if (tckn[0] == "0") return false;

        let toplam = 0;
        let toplamTek = 0;
        let toplamCift = 0;
        for (let i = 0; i < 10; i++) {
            toplam = toplam + parseInt(tckn[i]);
            if (i % 2 == 0) {
                // 11. basamak kontrolü yapılıyor
                if (i != 10) {
                    toplamTek = toplamTek + parseInt(tckn[i]); // 7 ile çarpılacak sayıları topluyoruz
                }
            }
            else {
                // 10. basamak toplanmıyor
                if (i != 9) {
                    toplamCift = toplamCift + parseInt(tckn[i]); // 10. basamak hariç çift sayıların toplamı
                }
            }
        }

        // 1.3.5.7.9. basamaklarının toplamının 7 katından,
        // 2.4.6.8. basamaklarının toplamını çıkartığımızda elde ettiğimiz sonucun
        // 10'a bölümünden kalan sayı (Mod 10), 10.basamaktaki sayı ile aynı olmalıdır.
        let dokuzundaHaneOlmasiGerekenDeger = ((toplamTek * 7) - toplamCift) % 10;
        if (dokuzundaHaneOlmasiGerekenDeger < 0) {
            dokuzundaHaneOlmasiGerekenDeger += 10;
        }

        if (dokuzundaHaneOlmasiGerekenDeger != parseInt(tckn[9])) return false;

        // ilk 10 hanenin toplamının 10’a bölümünden kalanın 11. haneyi vermesi gerekir.
        return (toplam % 10) == parseInt(tckn[10]);
    } catch (e) { }

    return false;
}
function validateTextInput(input) {
    input = $(input);

    const rules = input.data("validate")?.split(",");
    const errors = [];

    if (rules?.length > 0) {
        const inputTitle = input.data("title") || input.attr("placeholder") || " ";
        const inputType = input.prop("tagName").toLowerCase();
        const isMultiple = input.prop("multiple");
        const inputValue = isMultiple ? input.val() : input.val()?.trim();
        const inputValueFormatted = isMultiple ? 0 : inputValue.replaceAll(",", ".");

        //console.log(inputTitle, inputValueFormatted, IsNumeric(inputValueFormatted, true));

        for (const rule of rules) {
            const [name, value] = rule.split("=").map((item) => item.trim());

            if (name === "required" && (!(inputValue?.length > 0)
                || (inputType == "select" && !isMultiple && inputValue?.length == 0)
                || (inputType == "select" && isMultiple && inputValue?.length == 0)
            )) {
                errors.push(getErrorMessage(ERROR_MESSAGES.EMPTY_VALUE, inputTitle));
                return errors;
            }
            else if (name === "length" && inputValue?.length !== parseInt(value)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.EQUAL_LENGTH, inputTitle, value));
                return errors;
            }
            else if (name === "minlength" && inputValue?.length < parseInt(value)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MIN_LENGTH, inputTitle, value));
                return errors;
            }
            else if (name === "maxlength" && inputValue?.length > parseInt(value)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_LENGTH, inputTitle, value));
                return errors;
            }
            else if (name === "number" && !((inputValueFormatted == null || inputValueFormatted == '') || $.isNumeric(inputValueFormatted))) {
                errors.push(getErrorMessage(ERROR_MESSAGES.INVALID_VALUE, inputTitle));
                return errors;
            }
            else if (name === "minvalue" && (!IsNumeric(inputValueFormatted, true) || inputValueFormatted < parseFloat(value))) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MIN_VALUE, inputTitle, value));
                return errors;
            }
            else if (name === "maxvalue" && (!IsNumeric(inputValueFormatted, true) || inputValueFormatted > parseFloat(value))) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_VALUE, inputTitle, value));
                return errors;
            }
            else if (name === "tckn" && !validateTCKN(inputValue)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.INVALID_VALUE, inputTitle));
                return errors;
            }
            else if (name === "email" && !validateEmail(inputValue)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.INVALID_VALUE, inputTitle));
                return errors;
            }
        }
    }
    return errors;
}
function validateFileInput(input) {
    input = $(input);

    const inputTitle = input.data("title") || input.attr("placeholder") || " ";
    const rules = input.data("validate")?.split(",");
    const fileCount = input[0]?.files?.length;
    let maxFileSize = 0;

    $.each(input[0].files, (index, file) => {
        maxFileSize = Math.max(maxFileSize, file?.size / 1000);
    });

    const errors = [];

    if (rules?.length > 0) {
        for (const rule of rules) {
            const [name, value] = rule.split("=").map((item) => item.trim());

            if (name === "required" && fileCount === 0) {
                errors.push(getErrorMessage(ERROR_MESSAGES.EMPTY_VALUE, inputTitle));
            }
            else if (name === "maxcount" && fileCount > value) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_COUNT, inputTitle, value));
            }
            else if (name === "maxsize" && maxFileSize > (value * 1024)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_SIZE, inputTitle, value));
            }
            else if (maxFileSize > (CONSTANTS.DEFAULT_MAX_FILE_SIZE * 1024)) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_SIZE, inputTitle, CONSTANTS.DEFAULT_MAX_FILE_SIZE));
            }
            else if (fileCount > CONSTANTS.DEFAULT_MAX_FILE_COUNT) {
                errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_COUNT, inputTitle, CONSTANTS.DEFAULT_MAX_FILE_COUNT));
            }
        }
    }
    else if (maxFileSize > (CONSTANTS.DEFAULT_MAX_FILE_SIZE * 1024)) {
        errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_SIZE, inputTitle, CONSTANTS.DEFAULT_MAX_FILE_SIZE));
    }
    else if (fileCount > CONSTANTS.DEFAULT_MAX_FILE_COUNT) {
        errors.push(getErrorMessage(ERROR_MESSAGES.MAX_FILE_COUNT, inputTitle, CONSTANTS.DEFAULT_MAX_FILE_COUNT));
    }

    return errors;
}
function validateTableInput(input) {
    input = $(input);

    const inputTitle = input.data("title") || input.attr("placeholder") || " ";
    const rules = input.data("validate")?.split(",");
    const selectedRows = getSelectedRows(input);

    const errors = [];

    if (rules?.length > 0) {
        for (const rule of rules) {
            const [name, value] = rule.split("=").map((item) => item.trim());

            if (name === "required" && !selectedRows) {
                errors.push(getErrorMessage(ERROR_MESSAGES.PLEASE_SELECT, inputTitle));
                // return errors;
            }
        }
    }

    return errors;
}
function getFormInputs(formId) {
    if (!formId) return;

    const form = $(formId.startsWith("#") ? formId : `#${formId}`);
    const inputs = form.find("[data-validate]:visible, input[type='file']:visible");

    return Array.from(inputs);
}
function validateForm(formId, showErrors = true) {
    if (!formId) return;

    const inputs = getFormInputs(formId);

    let errors = [];

    inputs.forEach((input) => {
        const inputType = $(input).attr("type");
        if (inputType === "file") {
            errors = errors.concat(validateFileInput(input));
            return;
        }
        if ($(input).is("table")) {
            errors = errors.concat(validateTableInput(input));
            return;
        }
        errors = errors.concat(validateTextInput(input));
    });

    errors = errors.join("<br/>");

    if (showErrors && errors.length > 0) {
        toastr.error(errors);
        return false;
    }
    else if (errors.length > 0) {
        return errors;
    }

    return true;
}
function validateSingleInput(selector, showErrors = true) {
    if (!selector) return;

    const input = $(selector);

    let errors = [];

    const inputType = $(input).attr("type");

    if (inputType === "file") {
        errors = errors.concat(validateFileInput(input));
    }
    else if ($(input).is("table")) {
        errors = errors.concat(validateTableInput(input));
    }

    errors = errors.concat(validateTextInput(input)).join("<br/>");

    if (showErrors && errors.length > 0) {
        toastr.error(errors);
        return false;
    }
    else if (errors.length > 0) {
        return errors;
    }

    return true;
}

function firstLetterLowerCase(str) {
    return str?.charAt(0)?.toLowerCase() + str?.slice(1);
}

function onFileChange(selector, onSelectedCallBack, validateOnChange = true) {
    try {
        if (typeof selector === 'undefined') throw new Error("fileRefParam verisi  undefined hatası -> CSBFileUtill -> setFileName()");
        $(document).off("change", selector);
        $(document).on("change", selector, { ref: this }, function (e) {
            const $this = $(this);
            const key = firstLetterLowerCase($this.attr("name") ?? $this.attr("id"));
            if (!key) {
                return;
            }

            if (validateOnChange) {
                const validationErrors = validateFileInput($this);
                if (validationErrors.length > 0) {
                    toastr.error(validationErrors.join("<br/>"));
                    if (onSelectedCallBack) onSelectedCallBack(false);
                    return;
                }
            }

            const files = $this[0].files;
            const fileCount = files?.length;
            const isMultiple = $this.attr('multiple') ?? false;

            if (!(fileCount > 0)) {
                if (onSelectedCallBack) onSelectedCallBack(false);
                return;
            }

            let returnObj = {};
            let loopCount = 0;
            $.each(files, function (j, file) {
                const reader = new FileReader();
                reader.readAsDataURL(file);

                reader.onloadend = () => {
                    const base64String = reader.result.replace('data:', '').replace(/^.+,/, '');
                    let obj = {
                        dosyaUzanti: (file?.type?.split("/")[1] ? "." + file?.type?.split("/")[1] : ""),
                        dosyaBase64: base64String,
                        dosyaTurGuid: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                        name: file?.name,
                        size: file?.size,
                        type: file?.type,
                    };

                    if (!isMultiple) {
                        returnObj[key] = obj;
                        if (onSelectedCallBack) onSelectedCallBack(returnObj);
                        return;
                    }

                    if (!(key in returnObj)) {
                        returnObj[key] = [];
                    }

                    returnObj[key].push(obj);
                    loopCount++;
                    if (loopCount >= fileCount) {
                        if (onSelectedCallBack) onSelectedCallBack(returnObj);
                        return;
                    }
                };
            });
        });
    } catch (ex) {
        console.error(ex);
    }
};

function serializeFormAsObject(selector, ...args) {
    if (typeof selector === 'undefined') {
        return;
    }

    const $form = $(selector);
    let formObj = {};

    // select, inputlar, datetimelar (file, checkbox ve radiobuttonlar alınmamıştır.)
    $form.find('select, input[type!="file"][type!="checkbox"][type!="radio"]').not(":disabled").each(function () {
        const $this = $(this);
        const key = firstLetterLowerCase($this.attr("name") ?? $this.attr("id"));
        if (!key) {
            return;
        }

        const value = $this.val();
        formObj[key] = value;
    });

    // checkboxlar, radiobuttonlar
    $form.find('input[type="checkbox"]:checked, input[type="radio"]:checked').not(":disabled").each(function () {
        const $this = $(this);
        const key = firstLetterLowerCase($this.attr("name") ?? $this.attr("id"));
        if (!key) {
            return;
        }

        const type = $this.attr("type");
        const multiple = $this.attr("multiple");
        const value = $this.val();

        // radiobutton ise veya checkbox multiple prop' u yok ise tek bir adet seçilebileceği için doğrudan eklenir.
        if (type == 'radio' || !multiple) {
            formObj[key] = value;
            return;
        }
        // checkbox ise ve multiple prop' u varsa dizi olarak eklenecek.
        // halihazirda key yok ise oluşturulacak, varsa ekleniyor.
        if (!(key in formObj)) {
            formObj[key] = [];
        }
        formObj[key].push(value);
    });

    $form.find(".ckEditor").not(":disabled").each(function () {
        const $this = $(this);
        const key = firstLetterLowerCase($this.attr("name") ?? $this.attr("id"));
        if (!key || !window[key]) {
            return;
        }

        const value = window[key].getData();

        formObj[key] = value;
    });

    $form.find("textarea").not('.ckEditor').not(":disabled").each(function () {
        const $this = $(this);
        const key = firstLetterLowerCase($this.attr("name") ?? $this.attr("id"));
        if (!key) {
            return;
        }

        let value = $this.val();
        if (!value) value = $this.html();

        formObj[key] = value;
    });

    if (args?.length > 0) {
        for (let arg of args) {
            if (arg) {
                Object.assign(formObj, arg);
            }
        }
    }

    return formObj;
}

function formDataToQueryString(obj) {
    const stack = [{ object: obj, prefix: "" }];
    const params = new URLSearchParams();
    //let query = "";
    while (stack.length) {
        const { object, prefix } = stack.pop();

        // || typeof object === "number"
        if (typeof object === "string") {
            params.append(prefix, object);
            //query += prefix + "=" + object + "\r\n";
            continue;
        }

        let entries = Object.entries(object);
        if (!(entries?.length > 0)) {
            params.append(prefix, object);
            //query += prefix + "=" + object + "\r\n";
            continue;
        }

        for (const [key, value] of entries) {
            const newKey = prefix ? `${prefix}.${key}` : key;
            //Array.isArray(value) && typeof value !== "string"

            if (Array.isArray(value)) {
                value.forEach((item, index) => {
                    stack.push({ object: item, prefix: `${newKey}[${index}]` });
                });
            }
            else if (typeof value === "object" && value !== null) {
                stack.push({ object: value, prefix: newKey });
            }
            else {
                //query += newKey + "=" + value + "\r\n";
                params.append(newKey, value);
            }
        }
    }

    return "?" + params.toString();
}

function serializeFormToQueryString(selector) {
    const formData = serializeFormAsObject(selector);
    return formDataToQueryString(formData);
}

function renderCkEditor(selector) {
    return ClassicEditor.create(document.querySelector(selector));
}

function renderTooltip(placement = "top", container = "body") {
    $('[data-toggle="tooltip"]').tooltip({
        container: $(container),
        placement: placement,
    });
}

/* START OF DATATABLE */
function serverSideDataTableExportAction(e, dt, button, config) {
    let self = this;
    let oldStart = dt.settings()[0]._iDisplayStart;
    dt.one('preXhr', function (e, s, data) {
        data.start = 0;
        data.length = 2147483647;
        dt.one('preDraw', function (e, settings) {
            if (button[0].className.indexOf('buttons-copy') >= 0) {
                $.fn.dataTable.ext.buttons.copyHtml5.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-excel') >= 0) {
                $.fn.dataTable.ext.buttons.excelHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.excelHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.excelFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-csv') >= 0) {
                $.fn.dataTable.ext.buttons.csvHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.csvHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.csvFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-pdf') >= 0) {
                $.fn.dataTable.ext.buttons.pdfHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.pdfHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.pdfFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-print') >= 0) {
                setTimeout(function () { $.fn.dataTable.ext.buttons.print.action(e, dt, button, config); }, 0);
            }
            dt.one('preXhr', function (e, s, data) {
                settings._iDisplayStart = oldStart;
                data.start = oldStart;
            });
            setTimeout(dt.ajax.reload, 0);
            return false;
        });
    });
    dt.ajax.reload();
};

function renderTable(selector) {
    if (selector == undefined) selector = ".dataTable";
    $(selector).each(function () {
        const tableElem = $(this);
        const tableName = tableElem.attr("data-name");
        const tableDefaultLength = tableElem.attr("data-default-length") || 10;
        const serverSide = tableElem.attr("data-server-side") == "true";
        let tableColumns = [];
        let tableExportColumns = [];
        let disabledColumnSort = [];
        let tableExportButtons = [];
        let buttons = [];

        if (!serverSide) {
            try {
                tableElem.DataTable().destroy();
            } catch (e) { }
        }

        let tableHead = tableElem.find("thead tr");

        let selectableAttr = tableElem.data("selectable") ?? false;
        if (selectableAttr) {
            tableHead.prepend('<th data-name="sec" data-width="45px" data-sort="false">Seç</th>');
        }

        let showDetailsAttr = tableElem.data("showdetails")?.split(",").map((item) => item.trim()) ?? [];
        if (showDetailsAttr.includes("start")) {
            tableHead.prepend('<th data-name="showdetails" data-width="35px" data-sort="false"></th>');
        }
        if (showDetailsAttr.includes("end")) {
            tableHead.append('<th data-name="showdetails" data-width="35px" data-sort="false"></th>');
        }

        tableElem.find('thead tr th').each(function (i) {
            if ($(this).data('sort') == 'false')
                disabledColumnSort.push(i);

            if ($(this).data("export") != "false")
                tableExportColumns.push(i);

            let columnArr = {
                width: $(this).data("width") || "auto",
                className: $(this).attr('class') || "",
                data: $(this).data("name"),
                title: $(this).html(),
                sType: $(this).data("type") || "turkish",
                bSortable: $(this).data('sort') != 'false',
                bSearchable: true
            };

            tableColumns.push(columnArr);
        });

        if (tableElem.attr("data-export-buttons") == "true") {
            tableExportButtons.push(
                { extend: 'copyHtml5', text: '<span class="navi-icon"><i class="la la-copy"></i></span> Kopyala', footer: false, exportOptions: { columns: tableExportColumns } }
            );
            tableExportButtons.push(
                { extend: 'excel', text: '<span class="navi-icon"><i class="la la-file-excel-o"></i></span> Excel', footer: false, exportOptions: { columns: tableExportColumns } }
            );
            tableExportButtons.push(
                { extend: 'csvHtml5', text: '<span class="navi-icon"><i class="la la-file-text-o"></i></span> CSV', footer: false, exportOptions: { columns: tableExportColumns } }
            );
            //tableExportButtons.push(
            //    { extend: 'pdfHtml5', orientation: 'landscape', text: '<span class="navi-icon"><i class="la la-file-pdf-o"></i></span> PDF', footer: false, exportOptions: { columns: tableExportColumns } }
            //);
            tableExportButtons.push(
                { extend: 'print', text: '<span class="navi-icon"><i class="la la-print"></i></span> Yazdır', footer: false, exportOptions: { columns: tableExportColumns } }
            );

            if (serverSide) {
                for (let i = 0; i < tableExportButtons.length; i++) {
                    tableExportButtons[i].action = serverSideDataTableExportAction;
                    if (tableExportButtons[i].extend == "print") {
                        tableExportButtons[i].autoPrint = false;
                    }
                }
            }

            buttons.push(
                {
                    extend: 'collection',
                    text: '<i class="la la-download"></i>Dışarı Aktar',
                    buttons: tableExportButtons,
                    className: 'btn-sm'
                }
            );
        }

        let ajax = null;
        let ajaxUrl = tableElem.data("ajax-target");
        if (ajaxUrl != undefined) {
            var filterDataName = !IsNullOrEmpty(tableElem.attr("data-custom-filter-name")) ? ("[data-name='" + tableElem.attr("data-custom-filter-name") + "']") : `[data-name='${tableName}Filter']`;
            if (serverSide) {
                ajax = {
                    type: "POST",
                    url: ajaxUrl,
                    data: function (d) {
                        let formData = serializeFormAsObject(filterDataName);
                        Object.assign(d, formData);
                    },
                    beforeSend: function (request) {
                        if (tableName == "tableMalikler") {
                            startLoaderMalikler("[data-name='" + tableName + "']");
                        } else {
                            startLoader("[data-name='" + tableName + "']");
                        }
                    },
                    error: function (e) {
                        stopLoader("[data-name='" + tableName + "']");
                        toastr.error(getAjaxErrorMessage(e));
                        table.clear();
                        const $disableElements = $(filterDataName + " button");
                        $disableElements?.attr("disabled", false);
                        return false;
                    },
                    dataSrc: function (response) {
                        stopLoader("[data-name='" + tableName + "']");
                        if (typeof ajaxTableComplete === "function") {
                            return ajaxTableComplete(response?.data, tableElem);
                        } else {
                            return response?.data;
                        }
                    }
                }
            }
            else {
                ajax = {
                    type: "GET",
                    url: ajaxUrl,
                    contentType: 'application/json; charset=utf-8',
                    data: function (response) {
                        return response = JSON.stringify(response);
                    },
                    beforeSend: function (request) {
                        if (tableName == "tableMalikler") {
                            startLoaderMalikler("[data-name='" + tableName + "']");
                        } else {
                            startLoader("[data-name='" + tableName + "']");
                        }
                    },
                    error: function (e) {
                        stopLoader("[data-name='" + tableName + "']");
                        toastr.error(getAjaxErrorMessage(e));
                        table.clear().draw();
                        const $disableElements = $(filterDataName + " button");
                        $disableElements?.attr("disabled", false);
                        return false;
                    },
                    dataSrc: function (response) {
                        stopLoader("[data-name='" + tableName + "']");
                        if (typeof ajaxTableComplete === "function") {
                            return ajaxTableComplete(response, tableElem);
                        } else {
                            return response;
                        }
                    }
                }
            }
        }

        let tableOrder = [];
        let orderDefaultColumn = tableElem.attr("data-order-default-column");
        let orderDefaultType = tableElem.attr("data-order-default-type");
        if (orderDefaultColumn != undefined && orderDefaultType != undefined) {
            tableOrder = [orderDefaultColumn, orderDefaultType];
        }

        let columnDefs = [{ type: 'turkish', targets: '_all' }];

        let target = 0;
        if (showDetailsAttr.includes("start")) {
            columnDefs.push({ targets: target, defaultContent: "", orderable: false, className: "dt-control", width: "30px" });
            target++;

        }
        if (showDetailsAttr.includes("end")) {
            columnDefs.push({ targets: (tableColumns.length - 1), defaultContent: "", orderable: false, className: "dt-control control-end", width: "35px" });
        }

        if (selectableAttr) {
            columnDefs.push({ targets: target, defaultContent: "", className: "cbSec", width: "5%" });
        }

        let rowReorder = null;
        if (tableElem.data("rowreorder") != undefined) {
            let rowReorderColumnIndex = parseInt(tableElem.attr("data-rowreorder-column"));
            let rowReorderColumn = tableColumns[rowReorderColumnIndex];
            rowReorder = {
                dataSrc: rowReorderColumn.data
            };
        }

        let tableCustomButtonHtml = "";
        if (tableElem.data("custom-button") != undefined) {
            let tableCustomButtonText = tableElem.attr("data-custom-button");
            let tableCustomButtonFunction = tableElem.attr("data-custom-button-function");
            let tableCustomButtonParam = tableElem.attr("data-custom-button-param");
            let tableCustomButtonParam2 = tableElem.attr("data-custom-button-param2");

            if (IsNullOrEmpty(tableCustomButtonParam)) {
                tableCustomButtonHtml = `<button type="button" class="btn btn-sm btn-dark btn-table-custom-button" onclick="${tableCustomButtonFunction}();">${tableCustomButtonText}</button>`;
            } else {
                let _tableCustomButtonParam = $.isNumeric(tableCustomButtonParam) ? parseInt(tableCustomButtonParam) : `'${tableCustomButtonParam}'`;
                let _tableCustomButtonParam2 = $.isNumeric(tableCustomButtonParam2) ? parseInt(tableCustomButtonParam2) : `'${tableCustomButtonParam2}'`;
                if (!IsNullOrEmpty(_tableCustomButtonParam2) && _tableCustomButtonParam2 != 0) {
                    tableCustomButtonHtml = `<button type="button" class="btn btn-sm btn-dark btn-table-custom-button" onclick="${tableCustomButtonFunction}(${_tableCustomButtonParam}, ${_tableCustomButtonParam2});">${tableCustomButtonText}</button>`;
                } else {
                    tableCustomButtonHtml = `<button type="button" class="btn btn-sm btn-dark btn-table-custom-button" onclick="${tableCustomButtonFunction}(${_tableCustomButtonParam});">${tableCustomButtonText}</button>`;
                }
            }
        }

        let selectorAttr = tableElem.attr("data-selector") ?? ".cbSec";
        let selectProp = selectableAttr ? { style: selectableAttr, selector: selectorAttr } : false;
        let table = tableElem.DataTable({
            "dom": `<"row tableHeaderElem"<"col-md-6 customButton"><"col-md-6 tbl-search-area"Bf>>rt<"row tableBottomElem"<"col-sm-12 col-md-7"li><"col-sm-12 col-md-5 dataTables_pager"p>>`,
            ajax: ajax,
            select: selectProp,
            responsive: tableElem.attr("data-responsive") || false,
            serverSide: serverSide,
            searchDelay: 750,
            aoColumnDefs: [
                { type: 'turkish', targets: '_all', 'bSortable': false, 'aTargets': disabledColumnSort }
            ],
            autoWidth: false,
            rowId: tableElem.data("rowid") ?? "id",
            paging: tableElem.data("paging") || true,
            searching: tableElem.data("searching") || true,
            buttons: buttons,
            columnDefs: columnDefs,
            rowReorder: rowReorder,
            createdRow: function (row, data, index) {
                if (showDetailsAttr.includes("dblclick")) {
                    $(row).css({ "cursor": "pointer" });
                }

                if (data.color?.length > 0) {
                    $("td", row).css({
                        "background-color": data.color,
                        'box-shadow': 'inset 0 0 0 9999px var(--bs-table-striped-bg)'
                    });
                }
                if (data.textColor) {
                    $("td", row).css("color", data.textColor);
                }
                if (data.IsVisible == false) {
                    row.bgColor = '#d6d2d2'
                }
            },
            columns: tableColumns,
            "order": tableOrder,
            "iDisplayLength": parseInt(tableDefaultLength),
            initComplete: function () {
                if (typeof initCompleteTable === "function")
                    initCompleteTable(tableElem);

                renderTableTooltip(tableElem);
            },
            language: {
                search: "_INPUT_", searchPlaceholder: "Ara...",
                "emptyTable": "Tabloda herhangi bir veri mevcut değil",
                "info": "_TOTAL_ kayıttan _START_ - _END_ arası gösteriliyor",
                "infoEmpty": "Kayıt yok",
                "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
                "infoThousands": ".",
                "lengthMenu": "Sayfada _MENU_ kayıt göster",
                "loadingRecords": tableName == "tableMalikler" ? "Yükleniyor...Lütfen Yükleniyor...Lütfen bekleyiniz! Maliklerin listelenmesi uzun sürebilir!" : "Yükleniyor...",
                "processing": "İşleniyor...",
                "zeroRecords": "Eşleşen kayıt bulunamadı",
                "paginate": {
                    "first": "İlk",
                    "last": "Son",
                    //"next": "Sonraki",
                    //"previous": "Önceki"
                },
                "aria": {
                    "sortAscending": ": artan sütun sıralamasını aktifleştir",
                    "sortDescending": ": azalan sütun sıralamasını aktifleştir"
                },
                "select": {
                    "rows": {
                        "_": " %d kayıt seçildi",
                        "1": " 1 kayıt seçildi",
                        "0": " -"
                    }
                },
                "autoFill": {
                    "cancel": "İptal",
                    "fill": "Bütün hücreleri <i>%d<i> ile doldur<\/i><\/i>",
                    "fillHorizontal": "Hücreleri yatay olarak doldur",
                    "fillVertical": "Hücreleri dikey olarak doldur",
                    "info": "-"
                },
                "buttons": {
                    "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                    "colvis": "Sütun görünürlüğü",
                    "colvisRestore": "Görünürlüğü eski haline getir",
                    "copy": "Koyala",
                    "copyKeys": "Tablodaki sisteminize kopyalamak için CTRL veya u2318 + C tuşlarına basınız.",
                    "copySuccess": {
                        "1": "1 satır panoya kopyalandı",
                        "_": "%d satır panoya kopyalandı"
                    },
                    "copyTitle": "Panoya kopyala",
                    "csv": "CSV",
                    "excel": "Excel",
                    "pageLength": {
                        "-1": "Bütün satırları göster",
                        "1": "-",
                        "_": "%d satır göster"
                    },
                    "pdf": "PDF",
                    "print": "Yazdır"
                },
                "decimal": "-",
                "infoPostFix": "",
                "searchBuilder": {
                    "add": "Koşul Ekle",
                    "button": {
                        "0": "Arama Oluşturucu",
                        "_": "Arama Oluşturucu (%d)"
                    },
                    "clearAll": "Hepsini Kaldır",
                    "condition": "Koşul",
                    "conditions": {
                        "date": {
                            "after": "Sonra",
                            "before": "Önce",
                            "between": "Arasında",
                            "empty": "Boş",
                            "equals": "Eşittir",
                            "not": "Değildir",
                            "notBetween": "Dışında",
                            "notEmpty": "Dolu"
                        },
                        "moment": {
                            "after": "Sonra",
                            "before": "Önce",
                            "between": "Arasında",
                            "empty": "Boş",
                            "equals": "Eşittir",
                            "not": "Değildir",
                            "notBetween": "Dışında",
                            "notEmpty": "Dolu"
                        },
                        "number": {
                            "between": "Arasında",
                            "empty": "Boş",
                            "equals": "Eşittir",
                            "gt": "Büyüktür",
                            "gte": "Büyük eşittir",
                            "lt": "Küçüktür",
                            "lte": "Küçük eşittir",
                            "not": "Değildir",
                            "notBetween": "Dışında",
                            "notEmpty": "Dolu"
                        },
                        "string": {
                            "contains": "İçerir",
                            "empty": "Boş",
                            "endsWith": "İle biter",
                            "equals": "Eşittir",
                            "not": "Değildir",
                            "notEmpty": "Dolu",
                            "startsWith": "İle başlar"
                        }
                    },
                    "data": "Veri",
                    "deleteTitle": "Filtreleme kuralını silin",
                    "leftTitle": "Kriteri dışarı çıkart",
                    "logicAnd": "ve",
                    "logicOr": "veya",
                    "rightTitle": "Kriteri içeri al",
                    "title": {
                        "0": "Arama Oluşturucu",
                        "_": "Arama Oluşturucu (%d)"
                    },
                    "value": "Değer"
                },
                "searchPanes": {
                    "clearMessage": "Hepsini Temizle",
                    "collapse": {
                        "0": "Arama Bölmesi",
                        "_": "Arama Bölmesi (%d)"
                    },
                    "count": "{total}",
                    "countFiltered": "{shown}\/{total}",
                    "emptyPanes": "Arama Bölmesi yok",
                    "loadMessage": "Arama Bölmeleri yükleniyor ...",
                    "title": "Etkin filtreler - %d"
                },
                "searchPlaceholder": "Ara...",
                "thousands": "."
            }
        });

        table.off("row-reordered").on("row-reordered", function (e, diff, edit) {
            if (typeof rowReorderedTable === "function")
                rowReorderedTable(diff, tableElem);
        });

        tableElem.off("row-delete").on("row-delete", "tbody tr", function () {
            table.row($(this)).remove().draw();
        });

        if (showDetailsAttr.includes("dblclick")) {
            tableElem.off("dblclick").on("dblclick", "tr", function (e) {
                DatatableShowDetailsClickHandler(tableElem, $(this));
            });
        }
        if (showDetailsAttr.includes("start") || showDetailsAttr.includes("end")) {
            tableElem.off("click").on("click", "td.dt-control", function (e) {
                DatatableShowDetailsClickHandler(tableElem, $(this));
            });
        }

        table.off("page").on('page.dt', function (e) {
            if (typeof pageChangeTable === "function")
                pageChangeTable(tableElem);

            renderTableTooltip(tableElem);
        });

        if (table?.table()?.container() != undefined) {
            let tableHeaderElem = $(table.table().container()).find(".tableHeaderElem");
            tableHeaderElem.find(".customButton").html(tableCustomButtonHtml);
            if (IsNullOrEmpty(tableHeaderElem.text()) && tableHeaderElem.html().indexOf("<input") == -1 && tableHeaderElem.html().indexOf("<button") == -1 && tableHeaderElem.html().indexOf("<a") == -1) {
                tableHeaderElem.addClass("d-none");
            }
        }
    });
}

function DatatableShowDetailsClickHandler(tableElem, elem) {
    let row = getRow(tableElem, elem, false);
    if (!row) return;

    if (row.child.isShown()) {
        row.child.hide();
        return;
    }

    if (typeof showDetailsTable === "function") {
        row.child(showDetailsTable(row.data(), tableElem)).show();
        renderTableTooltip(tableElem);
    }
}

function isDataTable(tableSelector) {
    return $.fn.DataTable.isDataTable(tableSelector);
}

function loadTable(tableSelector, url) {
    if (isDataTable($(tableSelector))) {
        reloadTable(tableSelector);
    } else {
        $(tableSelector).attr("data-ajax-target", url);
        renderTable(tableSelector);
    }
}

function deselectRows(tableSelector) {
    $(tableSelector).DataTable().rows('.selected').nodes().to$().removeClass('selected');
}
function deselectRowByElem(tableSelector, elem, isElemChild = false) {
    var row = getRow(tableSelector, elem, false, isElemChild);
    if (!row) throw new Error("row verisi  undefined hatası -> CSBdatatableUtill -> deselectRowByCell()");

    row.deselect().draw(false);
}
function getRow(tableSelector, elem, getRowData = true, isElemChild = false) {
    if (isElemChild) elem = $(elem).closest('tr').parents('tr').prev('tr')[0];

    const elemType = $(elem).prop("tagName")?.toLowerCase() ?? "tr";

    let row = $(tableSelector).DataTable().row(elemType == "tr" ? elem : elem.parents('tr'));
    if (!(row?.length > 0)) return false;

    if (getRowData) return row.data() ?? false;
    return row;
}
function selectRowByElem({ tableSelector, elem, getRowData = true, isElemChild = false, deselectOthers = true }) {
    if (typeof elem === 'undefined') throw new Error("elem verisi  undefined hatası -> CSBdatatableUtill -> selectRowByElem()");

    // childlardan herhangi birinin butonuna tıklandığında parent' ini seçebilmek için bunu yazıyoruz.
    var row = getRow(tableSelector, elem, false, isElemChild);
    if (!row) return false;

    if (deselectOthers) deselectRows(tableSelector);

    $(row.node()).addClass('selected');

    if (getRowData) return row.data() ?? false;
    return row;
}
function selectMultipleRows(tableSelector, elems = [], deselectOthers = true) {
    let isSingleSelect = $(tableSelector).data("selectable") != "multiple";
    if (isSingleSelect) {
        selectRowByElem({ tableSelector: tableSelector, elem: elems, getRowData: false });
        return;
    }

    if (!(elems?.length > 0)) throw new Error("elems verisi  undefined hatası -> CSBdatatableUtill -> selectMultipleRows()");

    if (deselectOthers) deselectRows(tableSelector);

    for (let i = 0; i < elems.length; i++) {
        selectRowByElem({ tableSelector: tableSelector, elem: elems[i], getRowData: false, deselectOthers: false });
    }
}
function getSelectedRows(tableSelector, getRowData = true) {
    let isSingleSelect = $(tableSelector).data("selectable") != "multiple";

    if (isSingleSelect) {
        return (getRowData ? $(tableSelector).DataTable().row('.selected').data() : $(tableSelector).DataTable().row('.selected')) ?? false;
    }

    let rows = $(tableSelector).DataTable().rows('.selected');
    if (!getRowData) return rows?.length > 0 ? rows : false;

    let selectedList = rows.data()?.toArray();
    return selectedList?.length > 0 ? selectedList : false;
}

function reloadTable(tableSelector, keepState = true, highlightRowIds = [], resetPageNumber = false) {
    if (isDataTable($(tableSelector))) {
        const $elem = $(tableSelector);

        const $disableElements = $("[data-name='" + $elem.data("name") + "Filter'] button");
        $disableElements.attr("disabled", true);

        let parentRowIds = [];
        if (keepState)
            parentRowIds = $elem.find("tr.dt-hasChild").map(function () { return "#" + $(this).attr("id"); }).get();

        const sTable = $elem.DataTable();
        if (resetPageNumber) {
            sTable.ajax.reload(function (data) {
                if (typeof reloadTableFinish === "function")
                    reloadTableFinish($elem);

                renderTableTooltip($elem);
                $disableElements.attr("disabled", false);

                if (keepState) {
                    renderChildRows($elem, parentRowIds, highlightRowIds);
                }
                else {
                    highlightRows($elem, highlightRowIds);
                }
            });
        }
        else {
            sTable.ajax.reload(function (data) {
                if (typeof reloadTableFinish === "function")
                    reloadTableFinish($elem);

                renderTableTooltip($elem);
                $disableElements.attr("disabled", false);

                if (keepState) {
                    renderChildRows($elem, parentRowIds, highlightRowIds);
                }
                else {
                    highlightRows($elem, highlightRowIds);
                }
            }, false);
        }
    } else {
        $(tableSelector).attr("data-ajax-target", url);
        renderTable(tableSelector);
    }
}

function reloadTableNewAjaxUrl(tableSelector, ajaxUrl, keepState = true) {
    const $elem = $(tableSelector);

    const $disableElements = $("[data-name='" + $elem.data("name") + "Filter'] button");
    $disableElements.attr("disabled", true);

    let parentRowIds = [];
    if (keepState)
        parentRowIds = $elem.find("tr.dt-hasChild").map(function () { return "#" + $(this).attr("id"); }).get();

    const sTable = $elem.DataTable();
    sTable.ajax.url(ajaxUrl);
    sTable.ajax.reload(function (data) {
        if (typeof reloadTableFinish === "function")
            reloadTableFinish($(tableSelector));

        renderTableTooltip($(tableSelector));
        $disableElements.attr("disabled", false);

        if (keepState)
            renderChildRows($elem, parentRowIds);
    }, true);
}

function filterTable(tableSelector) {
    let sTable = $(tableSelector).DataTable();
    sTable.ajax.reload(function (data) {
        if (typeof filterTableFinish === "function")
            filterTableFinish($(tableSelector));

        renderTableTooltip($(tableSelector));
    }, true);
}

function clearTable(tableSelector) {
    if (isDataTable($(tableSelector))) {
        let sTable = $(tableSelector).DataTable();
        sTable.clear().draw();
    }
}

function destroyTable(tableSelector) {
    $(tableSelector).DataTable().destroy();
}

function renderTableTooltip(tableElem) {
    setTimeout(function () {
        let buttonElems = tableElem.find("[data-toggle='tooltip']:not([data-ui-tooltip])");
        let placement = buttonElems.data("placement") ?? "top";
        buttonElems.tooltip("dispose").tooltip({
            container: tableElem.parent().parent(),
            placement: placement
        });
    }, 200);
}

function renderChildRows(elem, parentRowIds, highlightRowIds = []) {
    if (!(parentRowIds?.length > 0)) return;
    setTimeout(function () {
        for (let i = 0; i < parentRowIds.length; i++) {
            $(elem).find(parentRowIds[i]).find("td.dt-control").click();
        }
        highlightRows(elem, highlightRowIds);
    }, 200);
}
function highlightRows(elem, highlightRowIds) {
    if (!(highlightRowIds?.length > 0)) return;
    //setTimeout(function () {
    //    for (let i = 0; i < highlightRowIds.length; i++) {
    //        $row = $(elem).find("[data-id='" + highlightRowIds[i] + "']");
    //        console.log($(elem) , $row);
    //        //$(elem).find("[data-id='" + highlightRowIds[i] + "']").css('color', 'red').animate({ color: 'black' }, { duration: 8000 });
    //    }
    //}, 200);
}

//datatable row format order extension
jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "formatted-num-pre": function (a) {
        a = (a == "-" || a == "" || a == null) ? 0 : a.toString().replace(".", "").replace(",", ".").replace(/[^\d\-\.]/g, "").replace("--", "|").replace("-", "").replace("|", "-");
        return parseFloat(a);
    },
    "formatted-num-asc": function (a, b) {
        return a - b;
    },
    "formatted-num-desc": function (a, b) {
        return b - a;
    },
    "turkish-pre": function (a) {
        let special_letters = {
            "C": "Ca", "c": "ca", "Ç": "Cb", "ç": "cb",
            "G": "Ga", "g": "ga", "Ğ": "Gb", "ğ": "gb",
            "I": "Ia", "ı": "ia", "İ": "Ib", "i": "ib",
            "O": "Oa", "o": "oa", "Ö": "Ob", "ö": "ob",
            "S": "Sa", "s": "sa", "Ş": "Sb", "ş": "sb",
            "U": "Ua", "u": "ua", "Ü": "Ub", "ü": "ub"
        };
        if (a == null || a == '') {
            return '';
        } else {
            for (let val in special_letters)
                a = a.toString().split(val).join(special_letters[val]).toLowerCase();
            return a;
        }
    },
    "turkish-asc": function (a, b) {
        return (a < b) ? -1 : ((a > b) ? 1 : 0);
    },
    "turkish-desc": function (a, b) {
        return (a < b) ? 1 : ((a > b) ? -1 : 0);
    }
});
/* END OF DATATABLE */

function confirmFunction(selector) {
    if (selector == undefined) selector = "*[data-confirm=true]";
    $(document).on("click", selector, function (e) {
        let $this = $(this);
        e.preventDefault();

        Swal.fire({
            title: $this.attr("data-confirm-title") || "Emin misiniz?",
            text: $this.attr("data-confirm-text") || "Bu işlem geri alınamayacaktır!",
            icon: $this.attr("data-confirm-icon") || "warning",
            html: $this.attr("data-html") || "",
            showCancelButton: true,
            cancelButtonText: $this.attr("data-confirm-cancel-button-text") || "Vazgeç",
            confirmButtonColor: $this.attr("data-confirm-confirm-button-color") || "#DD6B55",
            confirmButtonText: $this.attr("data-confirm-confirm-button-text") || "Evet",
            preConfirm: (pno) => {
                const preConfirm = $this.attr("data-preconfirm-function") || false;
                if (preConfirm) {
                    let param1 = $this.attr("data-confirm-param");
                    let param2 = $this.attr("data-confirm-param2");
                    let param3 = $this.attr("data-confirm-param3");
                    if (param1 != undefined && param2 != undefined && param3 != undefined) {
                        return window[preConfirm](param1, param2, param3);
                    } else if (param1 != undefined && param2 != undefined) {
                        return window[preConfirm](param1, param2);
                    } else if (param1 != undefined) {
                        return window[preConfirm](param1);
                    } else {
                        return window[preConfirm]();
                    }
                }
            }
        }).then((result) => {
            if (result.isConfirmed) {
                let param1 = $this.attr("data-confirm-param");
                let param2 = $this.attr("data-confirm-param2");
                let param3 = $this.attr("data-confirm-param3");
                if (param1 != undefined && param2 != undefined && param3 != undefined) {
                    window[$this.attr("data-confirm-function")](param1, param2, param3);
                } else if (param1 != undefined && param2 != undefined) {
                    window[$this.attr("data-confirm-function")](param1, param2);
                } else if (param1 != undefined) {
                    window[$this.attr("data-confirm-function")](param1);
                } else {
                    window[$this.attr("data-confirm-function")]();
                }
            }
        });
    });
}

function fireConfirmSwal(args) {
    if (!args) return;

    Swal.fire({
        title: args.title || "Emin misiniz?",
        text: args.text || "Bu işlem geri alınamayacaktır!",
        icon: args.icon || "warning",
        showCancelButton: true,
        cancelButtonText: args.cancelText || "Vazgeç",
        confirmButtonColor: args.confirmColor || "#DD6B55",
        confirmButtonText: args.confirmText || "Evet",
    }).then((result) => {
        if (result.isConfirmed) {
            let param1 = args.param1;
            let param2 = args.param2;
            let param3 = args.param3;
            if (param1 != undefined && param2 != undefined && param3 != undefined) {
                window[args.onconfirm](param1, param2, param3);
            }
            else if (param1 != undefined && param2 != undefined) {
                window[args.onconfirm](param1, param2);
            }
            else if (param1 != undefined) {
                window[args.onconfirm](param1);
            }
            else {
                window[args.onconfirm]();
            }
        }
    });

}

function renderSelect2(selector) {
    if (selector == undefined) selector = '.select-2';
    $(selector).each(function () {
        let $this = $(this);

        let isReadOnly = $this.attr("readonly") ?? false;
        let selectedValue = $this.attr("data-value") || "";
        let placeholder = $this.attr("placeholder") || "Seçilmedi";
        let cClass = $this.attr("custom-class") || "";

        let dropdownParent = $(document.body);
        const parent = $this.parents('.modal-content:first');
        if (parent.length > 0)
            dropdownParent = parent;

        if ($this[0].hasAttribute("data-ajax-target")) {
            Request($this.attr("data-ajax-target"), null, "GET").done(function (response) {
                let selectData = $.map(response, function (obj) {
                    if (obj.IsVisible != undefined && obj.IsVisible == 0) {
                        return;
                    }

                    let id = '';
                    if ($this[0].hasAttribute("data-custom-value")) {
                        id = obj[$this.attr("data-custom-value")];
                    } else {
                        id = obj.id;
                    }

                    let text = '';
                    if ($this[0].hasAttribute("data-custom-text")) {
                        text = obj[$this.attr("data-custom-text")];
                    } else {
                        if (obj.ad != undefined)
                            text = obj.ad;
                    }

                    return { id: id, text: text };
                });

                $this.select2({
                    width: '100%',
                    placeholder: placeholder,
                    dropdownParent: dropdownParent,
                    allowClear: true,
                    containerCssClass: cClass,
                    language: {
                        noResults: function () {
                            return "Sonuç bulunamadı";
                        }
                    },
                    data: selectData
                });

                if (selectedValue != undefined) {
                    if ($this.attr("multiple") != undefined) {
                        $this.val(selectedValue.split(',')).change();
                    } else {
                        $this.val(selectedValue).change();
                    };
                } else {
                    $this.val(selectedValue).change();
                }

                //if (typeof RenderSelectComplete === "function") {
                //    return RenderSelectComplete(response?.data, $this);
                //}

                if (isReadOnly) {
                    setSelectReadonly($this);
                }
                else if ($this.attr("name") == "UavtIlNo" && window.kullanici.birimIlId > 0) {
                    $this.val(window.kullanici.birimIlId).change();
                    setSelectReadonly($this);
                }

                let completeFunction = $this.attr("data-ajax-complete");
                if (completeFunction != undefined) {
                    window[completeFunction](response, $this);
                }
            });
        }
        else {
            $this.select2({
                width: '100%',
                placeholder: placeholder,
                containerCssClass: cClass,
                dropdownParent: dropdownParent,
                allowClear: true,
                language: {
                    noResults: function () {
                        return "Sonuç bulunamadı";
                    }
                },
            });
            if (selectedValue)
                $this.val(selectedValue).change();
        }
    });
}

function fillSelect(selector, url, selectedValue, successCallback, errorCallback) {
    let isReadOnly = $(selector).attr("readonly") ?? false;
    let output = '<option></option>';
    let customValue = $(selector).attr("data-custom-value") ?? "id";
    let customText = $(selector).attr("data-custom-text") ?? "ad";

    $(selector).html(output).change();

    Request(url, null, "GET")
        .done(function (response) {
            $.each(response, function (index, row) {
                output += '<option value="' + row[customValue] + '">' + row[customText] + '</option>';
            });
            $(selector).html(output);
            if (!IsNullOrEmpty(selectedValue)) {
                $(selector).val(selectedValue).attr("data-value", selectedValue).change();
            } else if (!IsNullOrEmpty($(selector).attr("data-value"))) {
                $(selector).val($(selector).attr("data-value")).change();
            } else {
                $(selector).change();
            }

            if (isReadOnly) {
                setSelectReadonly(selector);
            }
            if (successCallback != undefined) {
                successCallback();
            }
        }).fail(function (jqXHR, textStatus) {
            if (errorCallback != undefined) {
                errorCallback(jqXHR);
            }
        });
}

function fillSelect2(selector, url, selectedValue, successCallback, errorCallback) {
    let isReadOnly = $(selector).attr("readonly") ?? false;
    let output = '<option></option>';
    let customValue = $(selector).attr("data-custom-value") ?? "id";
    let customText = $(selector).attr("data-custom-text") ?? "ad";

    $(selector).html(output).change();

    Request(url, null, "GET")
        .done(function (response) {
            $.each(response, function (index, row) {
                output += '<option value="' + row[customValue] + '">' + row[customText] + '</option>';
            });
            $(selector).html(output);

            $(selector).val($(selector).attr("data-value")).change();

            if (isReadOnly) {
                setSelectReadonly(selector);
            }
            if (successCallback != undefined) {
                successCallback();
            }
        }).fail(function (jqXHR, textStatus) {
            if (errorCallback != undefined) {
                errorCallback(jqXHR);
            }
        });
}

function fillForm($form, data, callback) {
    $.each(data, function (key, value) {
        if (value != null) {
            if ($.type(value) == "object") {
                $.each(value, function (subKey, subValue) {
                    if (subValue != null) {
                        let subIsMultiple = $form.find("[name='" + key + "." + subKey + "']").is("[multiple]");
                        if (typeof subIsMultiple !== typeof undefined && subIsMultiple !== false && subValue != undefined && subValue != null) {
                            $form.find("[name='" + key + "." + subKey + "']").val(subValue.toString().split(',')).change();
                        }
                        else {
                            if (subValue.toString().length == 25 && subValue.toString().indexOf("T") == 10 && subValue.toString().indexOf("+") == 19) { //datetime
                                subValue = subValue.toString().substr(0, subValue.toString().indexOf("+"));
                            }
                            if ($form.find("[name='" + key + "." + subKey + "']").is("select")) {
                                $form.find("[name='" + key + "." + subKey + "']").val(subValue.toString()).attr("data-value", subValue.toString()).change();
                            } else if ($form.find("[name='" + key + "." + subKey + "']").hasClass("summernote")) {
                                $form.find("[name='" + key + "." + subKey + "']").summernote('code', subValue.toString());
                            } else {
                                $form.find("[name='" + key + "." + subKey + "']").val(subValue.toString()).change();
                            }
                        }
                    }
                });
            }
            let isMultiple = $form.find("[name='" + key + "']").is("[multiple]");
            if (typeof isMultiple !== typeof undefined && isMultiple !== false && value != undefined && value != null) {
                $form.find("[name='" + key + "']").val(value.toString().split(',')).change();
            }
            else {
                if (value.toString().length == 25 && value.toString().indexOf("T") == 10 && value.toString().indexOf("+") == 19) { //datetime
                    value = value.toString().substr(0, value.toString().indexOf("+"));
                }
                if ($form.find("[name='" + key + "']").is("select")) {
                    $form.find("[name='" + key + "']").val(value.toString()).attr("data-value", value.toString()).change();
                } else if ($form.find("[name='" + key + "']").hasClass("summernote")) {
                    $form.find("[name='" + key + "']").summernote('code', value.toString());
                } else {
                    let type = $form.find("[name='" + key + "']").attr("type");
                    if (type != 'file') {
                        $form.find("[name='" + key + "']").val(value.toString()).change();
                    }
                }
            }
        }
    });
    if (callback != undefined) {
        callback.call();
    }
}

function resetForm($form, callback) {
    $($form).find("input:not([type='checkbox']):not([type='radio']), select:not([readonly='readonly']), textarea")
        .val('')
        .find("[type!='file']").change();

    if (callback != undefined) {
        callback.call();
    }
}

function setSelectReadonly(elem, state = true) {
    let span = $(elem).parent().find(".select2-container");

    if (state) {
        $(elem).addClass("readonly");
        $(elem).attr("readonly", "readonly");
        span.addClass("readonly");
    }
    else {
        $(elem).removeAttr("readonly");
        $(elem).removeClass("readonly");
        span.removeClass("readonly");
    }
}

// remove array item
Array.prototype.remove = function () {
    let what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};

function dateToString(value) {
    if (IsNullOrEmpty(value))
        return '';

    if (value.indexOf("+") > 0)
        value = value.substr(0, value.indexOf("+"));

    return moment(value).format(`${window.culture?.shortDatePattern ?? 'DD/MM/yyyy'} HH:mm`).replaceAll("Invalid date", "");
}

function dateToDate(value) {
    if (IsNullOrEmpty(value))
        return '';

    if (value.indexOf("+") > 0)
        value = value.substr(0, value.indexOf("+"));

    return moment(value).format(window.culture?.shortDatePattern ?? 'DD/MM/yyyy').replaceAll("Invalid date", "");
}

function dateToFormat(value, format) {
    return moment(value).format(format).replaceAll("Invalid date", "");
}

function dateToDateGlobal(value) {
    return moment(value).format("YYYY-MM-DD").replaceAll("Invalid date", "");
}

function clearHtmlTag(text) {
    return text.replaceAll("<br />", " ").replace(/(<([^>]+)>)/ig, "");
}

function stringToSlug(text) {
    if (IsNullOrEmpty(text))
        return '';

    let trMap = {
        'çÇ': 'c',
        'ğĞ': 'g',
        'şŞ': 's',
        'üÜ': 'u',
        'ıİ': 'i',
        'öÖ': 'o'
    };

    for (let key in trMap) {
        text = text.toString().replace(new RegExp('[' + key + ']', 'g'), trMap[key]);
    }

    return text.replace(/[^-a-zA-Z0-9\s]+/ig, '')
        .replace(/\s/gi, "-")
        .replace(/[-]+/gi, "-")
        .toLowerCase();
}

function IsNullOrEmpty(text) {
    text = text?.toString() ?? '';
    return !(text?.length > 0);
}

function generateString(length) {
    let text = "";
    let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (let i = 0; i < length; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

function Request(url, data, type) {
    return $.ajax({
        type: type,
        url: url,
        data: data,
        dataType: 'json',
        cache: false
    });
}

function RequestText(url, data, type) {
    return $.ajax({
        type: type,
        url: url,
        data: data,
        dataType: 'text',
        cache: false
    });
}

function RequestJSON(Url, Data, Method) {
    return $.ajax({
        url: Url,
        type: Method,
        dataType: 'json',
        contentType: "application/json",
        data: Data,
        cache: false
    });
}

function RequestMultiPart(url, data, type) {
    return $.ajax({
        url: url,
        type: type,
        data: data,
        cache: false,
        contentType: false,
        processData: false
    });
}

function startLoader(selector) {
    let elem = $(selector).find('div.modal-content').length > 0 ? $(selector).find('div.modal-content') : ($(selector).closest('div.card').length > 0 ? $(selector).closest('div.card') : $(selector));
    elem.addClass("overlay overlay-block");
    elem.append('<div class="overlay-layer card-rounded bg-dark bg-opacity-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>');
}
function startLoaderMalikler(selector) {
    let elem = $(selector).find('div.modal-content').length > 0 ? $(selector).find('div.modal-content') : ($(selector).closest('div.card').length > 0 ? $(selector).closest('div.card') : $(selector));
    elem.addClass("overlay overlay-block");
    elem.append(`<div class="overlay-layer card-rounded bg-dark bg-opacity-5" style="top:-150px !important; ">                
                <div class="ml-10 alert alert-warning" role="alert">
                <div class="spinner-border text-primary" role="status" style="display:block; margin:auto; margin-bottom:10px;"></div>
                    Yükleniyor...Lütfen bekleyiniz! Maliklerin listelenmesi uzun sürebilir! Başka işlem yapmayınız!
                </div>
                </div>
                `);
}

function stopLoader(selector) {
    let elem = $(selector).find('div.modal-content').length > 0 ? $(selector).find('div.modal-content') : ($(selector).closest('div.card').length > 0 ? $(selector).closest('div.card') : $(selector));
    elem.removeClass("overlay overlay-block");
    elem.find("div.overlay-layer").remove();
}

function getAjaxErrorMessage(jqXHR) {
    switch (jqXHR?.status) {
        case 400:
            return IsNullOrEmpty(jqXHR?.responseText) ? "İşlem yapılırken hata oluştu!" : jqXHR.responseText.replaceAll("\n", "<br />");
        case 404:
            return "İşlem yapılacak sayfa bulunamadı!";
        case 403:
            return "İşlem yapmaya yetkiniz bulunmamaktadır!";
        case 401:
            return "İşlem yapmak için giriş yapmanız gerekmektedir!";
        case 500:
            return "Sunucu hatası oluştu!";
        default:
            return "İşlem yapılırken bilinmeyen bir hata oluştu!";
    }
}

function IsNumeric(number, trueIfNull = false) {
    if (IsNullOrEmpty(number)) return trueIfNull;

    number = number.toString().replaceAll(",", ".");
    if (isNaN(number)) return false;
    return parseFloat(number);
}

function toNumberFormat(number, decimalCount = null) {
    decimalCount ??= window.culture?.numberDecimalDigits || 2;
    const numberParsed = IsNumeric(number);
    return numberParsed ? numberParsed.toFixed(decimalCount).toString()
        .replaceAll(".", window.culture?.numberDecimalSeparator || ",")
        .replaceAll(",", window.culture?.numberDecimalSeparator || ",")
        .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") : number;
}

function toTurkishLira(money, decimalCount = null) {
    decimalCount ??= window.culture?.numberDecimalDigits || 2;
    const numberParsed = IsNumeric(money);
    return numberParsed ? `${toNumberFormat(money, decimalCount)} ${window.culture.currencySymbol}` : null;
}

function toTurkishLiraToNumber(money, decimalCount = null) {
    const moneyParsed = Number(money.replaceAll(".", "").replaceAll(window.culture.currencySymbol, "").replaceAll(",", "."))
    return moneyParsed;
}

function toPercentage(percentage) {
    return IsNumeric(percentage) ? "% " + percentage.toString() : null;
}