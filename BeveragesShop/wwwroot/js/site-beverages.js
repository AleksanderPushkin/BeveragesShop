// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


$('.btn-number').click(function (e) {
    e.preventDefault();

    

    fieldName = $(this).attr('data-field');
    parentFieldName = $(this).attr('data-parent');
    maxQty = parseInt($(this).attr('max'));
    count = parseInt($(this).attr('count'));
    type = $(this).attr('data-type');
    value = parseInt($(this).attr('data-value'));
    var currentCount = $("[name='" + parentFieldName + "']");
    
    var maxCount = $("[name='" + parentFieldName + "']");
    var input = $("[name='" + fieldName + "']");
    var currentVal = parseInt(input.text());
    if (!isNaN(currentVal)) {
        if (type == 'minus') {

            if (currentVal > input.attr('min')) {
                currentCount.val(parseInt(currentCount.val()) - 1);
             
                input.text(currentVal - value).change();
            }
            if (parseInt(input.text()) == input.attr('min')) {
                $(this).prop('disabled', true).addClass('btn-disabled');
            }

        } else if (type == 'plus') {

            if (currentVal < input.attr('max')) {
                currentCount.val(parseInt(currentCount.val()) + 1);
               
                if (parseInt(currentCount.val()) + count >= maxQty) {
                    $(this).prop('disabled', true).addClass('btn-disabled');
                }
                input.text(currentVal + value).change();
                
            }
            if (parseInt(input.val()) == input.attr('max')) {
                $(this).prop('disabled', true).addClass('btn-disabled');
            }

        }
    } else {
        input.text(0);
    }

});
$('.input-number').focusin(function () {
    $(this).data('oldValue', $(this).text());
});
$('.input-number').change(function () {

    minValue = parseInt($(this).attr('min'));
    maxValue = parseInt($(this).attr('max'));
    valueCurrent = parseInt($(this).text());

    name = $(this).attr('name');
    if (valueCurrent >= minValue) {
        $(".btn-number[data-type='minus'][data-field='" + name + "']").not('.btn-disabled').removeAttr('disabled')
    } else {
        alert('Sorry, the minimum value was reached');
        $(this).text($(this).data('oldValue'));
    }
    if (valueCurrent <= maxValue) {
        $(".btn-number[data-type='plus'][data-field='" + name + "']").not('.btn-disabled').removeAttr('disabled')
    } else {
        alert('Sorry, the maximum value was reached');
        $(this).text($(this).data('oldValue'));
    }

    Recalculate();
});
$(".input-number").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});


$('.btn-select').click(function (e) {
    e.preventDefault();
    if ($(this).hasClass("beverage-pricenotok")) return;


    var input = $("[name='BeverageCost']");
    input.text($(this).attr('price'));


    $("input[name='BeverageId']").val($(this).attr('BeverageId'));
    $("input[name='Count']").val(1);
    $("input[name='Price']").val($(this).attr('Price'));


   

    Recalculate();
});

function Recalculate() {
    var changeMoney = $("[name='ChangeMoney']");
    var totalMoney = $("[name='TotalMoney']");
    var beverageCost = $("[name='BeverageCost']");
    changeMoney.text(parseInt(totalMoney.text()) - parseInt(beverageCost.text()));

    var money = $('#Money');
    money.val(parseInt(totalMoney.text()));
    $("input[name='Change']").val(changeMoney.text());



    if (parseInt(changeMoney.text()) < 0 || parseInt(beverageCost.text()) == 0) {
        $('.btn-buy').prop("disabled", true);
    }
    else {
        $('.btn-buy').prop("disabled", false);
    }


    $("div[name='beverages']").not(".not-selectable").prop('disabled', false);

    $("div[name='beverages']").not(".not-selectable").filter(function () {
        return parseInt($(this).attr('price')) <= parseInt(totalMoney.text())
    }).prop('disabled', false).removeClass("beverage-pricenotok").addClass("beverage-priceok");

    $("div[name='beverages']").not(".not-selectable").filter(function () {
        return parseInt($(this).attr('price')) > parseInt(totalMoney.text())
    }).prop('disabled', true).removeClass("beverage-priceok").addClass("beverage-pricenotok");;
}

Recalculate();