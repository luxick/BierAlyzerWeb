// #region ValidateDoubleTextBox

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>   Keeps an input value a double </summary>
///
/// <remarks>   Andre Beging, 28.04.2018. </remarks>
///
/// <param name="input">    The input. </param>
/// <param name="e">        The unknown to process. </param>
///
/// <returns>   . </returns>
////////////////////////////////////////////////////////////////////////////////////////////////////
function ValidateDoubleTextBox(input, e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }

    var z = "penis";
    var newText = input.value + e.key;
    newText = newText.replace(",", ".");

    if (isNaN(newText)) {
        e.preventDefault();
        return;
    } else {
    // catch dot
        if (e.keyCode === 190) {
            e.preventDefault();
            input.value = newText;
        }
    }
}

// #endregion