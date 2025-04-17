// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to calculate total price (quantity * unit price)
function calculateTotalPrice(row) {
    const quantity = parseFloat(row.find('input[name$=".Quantity"]').val()) || 0;
    const unitPrice = parseFloat(row.find('input[name$=".UnitPrice"]').val()) || 0;
    const totalPrice = quantity * unitPrice;
    row.find('input[name$=".TotalPrice"]').val(totalPrice.toFixed(2));
}

// Add event listeners for quantity and unit price changes
$(document).on('input', 'input[name$=".Quantity"], input[name$=".UnitPrice"]', function() {
    const row = $(this).closest('tr');
    calculateTotalPrice(row);
});

// Function to handle the Remove button click event
$(document).on('click', '.remove-line', function() {
    console.log('Remove button clicked');
    const row = $(this).closest('tr');
    
    // Extract purchase ID from URL
    const urlParts = window.location.pathname.split('/');
    const purchaseId = urlParts[urlParts.length - 1];
    
    // Extract sequence ID from data attribute or input field names
    let sequenceId = 0;
    if (typeof row.data('sequence-id') !== 'undefined') {
        sequenceId = parseInt(row.data('sequence-id'));
        console.log('Found sequenceId from data attribute:', sequenceId);
    } else {
        // Fallback to extracting from input fields
        const descInput = row.find('input[name$=".Description"]');
        if (descInput.length) {
            const nameAttr = descInput.attr('name');
            if (nameAttr) {
                const match = nameAttr.match(/Lines\[(\d+)\]/);
                if (match && match[1]) {
                    sequenceId = parseInt(match[1]);
                }
            }
        }
    }
    
    console.log('Removing invoice line with purchaseId:', purchaseId, 'and sequenceId:', sequenceId);
    
    if (confirm('Are you sure you want to delete this invoice line?')) {
        $.ajax({
            url: '/PurchaseInvoiceHeaders/DeleteInvoiceLine',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                purchaseId: parseInt(purchaseId),
                sequenceId: sequenceId
            }),
            success: function(response) {
                console.log('Server response:', response);
                if (response && response.success) {
                    alert(response.message || 'Line deleted successfully.');
                    // Reload the page to refresh the table
                    window.location.reload();
                } else {
                    alert('Failed to delete the line: ' + (response && response.message ? response.message : 'Unknown error'));
                }
            },
            error: function(xhr, status, error) {
                console.error('AJAX error:', xhr);
                console.error('Status:', status);
                console.error('Error:', error);
                console.error('Response text:', xhr.responseText);
                alert('An error occurred while deleting the line. See console for details.');
            }
        });
    }
});

// Function to handle the Save button click event
$(document).on('click', '.save-line', function () {
    console.log('Save button clicked');
    const row = $(this).closest('tr');
    console.log('Row data:', row);
      // Try to get purchaseId from the URL if not available in data attribute
    let purchaseId = row.data('purchase-id');
    if (!purchaseId) {
        // Extract from URL: http://localhost:5096/PurchaseInvoiceHeaders/Edit/2
        const urlParts = window.location.pathname.split('/');
        purchaseId = urlParts[urlParts.length - 1];
    }
    
    console.log('Purchase ID:', purchaseId);    // Get the sequenceId for this invoice line
    let sequenceId = 0;
    
    // Debug: Dump all inputs in this row to see their names
    console.log('All inputs in this row:');
    row.find('input').each(function() {
        console.log($(this).attr('name'), '=', $(this).val());
    });
    
    // Add a data attribute in the HTML to store the sequenceId directly
    if (typeof row.data('sequence-id') !== 'undefined') {
        sequenceId = parseInt(row.data('sequence-id'));
        console.log('Found sequenceId from data attribute:', sequenceId);
    } else {
        // As fallback, try to extract from input names
        const descInput = row.find('input[name$=".Description"]');
        if (descInput.length) {
            const nameAttr = descInput.attr('name');
            console.log('Description input name:', nameAttr);
            if (nameAttr) {
                const match = nameAttr.match(/Lines\[(\d+)\]/);
                if (match && match[1]) {
                    sequenceId = parseInt(match[1]);
                    console.log('Extracted sequenceId from Description input:', sequenceId);
                }
            }
        }
    }
    
    console.log('Final Sequence ID to be used:', sequenceId);
    
    // Calculate total price as quantity * unit price
    const quantity = parseFloat(row.find('input[name$=".Quantity"]').val());
    const unitPrice = parseFloat(row.find('input[name$=".UnitPrice"]').val());
    const totalPrice = quantity * unitPrice;
      // Ensure we have a valid itemId to prevent NULL values in the database
    const itemId = row.find('input[name$=".ItemId"]').val();
    if (!itemId) {
        alert('Item ID is required. Please enter a valid Item ID.');
        console.error('Missing Item ID');
        return;
    }
      
    // Update the total price field in the UI
    row.find('input[name$=".TotalPrice"]').val(totalPrice.toFixed(2));
        
    const lineData = {
        purchaseId: parseInt(purchaseId),
        sequenceId: sequenceId,
        description: row.find('input[name$=".Description"]').val(),
        quantity: quantity,
        unitPrice: unitPrice,
        totalPrice: totalPrice,
        itemId: itemId, // Use the validated itemId directly
      };    
      console.log('Line data:', lineData);

    // Validate data
    if (!lineData.purchaseId || !lineData.description || isNaN(lineData.quantity) || isNaN(lineData.unitPrice)) {
        alert('Please fill in all fields correctly.');
        console.error('Validation failed:', lineData);
        return;
    }

    // Send data to the server
    $.ajax({
        url: '/PurchaseInvoiceHeaders/SaveInvoiceLine',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(lineData),
        beforeSend: function() {
            console.log('Sending AJAX request to /PurchaseInvoiceHeaders/SaveInvoiceLine');
            console.log('Request payload:', JSON.stringify(lineData));
        },
        success: function (response) {
            console.log('Server response:', response);
            if (response && response.success) {
                alert(response.message || 'Line saved successfully.');
                // Reload the page to refresh the table
                window.location.reload();
            } else {
                alert('Failed to save the line: ' + (response && response.message ? response.message : 'Unknown error'));
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX error:', xhr);
            console.error('Status:', status);
            console.error('Error:', error);
            console.error('Response text:', xhr.responseText);
            alert('An error occurred while saving the line. See console for details.');
        }
    });
});
