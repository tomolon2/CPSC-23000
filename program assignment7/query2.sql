SELECT i.invoice_id AS InvoiceNumber, l.line_no AS LineNumber, l.product_code AS ProductCode, p.description AS ProductDescription,
       l.quantity AS LineUnits, l.unit_price AS LinePrice, (l.quantity * l.unit_price) AS LineTotal
FROM Invoice i
JOIN Line l ON l.invoice_id = i.invoice_id
LEFT JOIN Product p ON p.product_code = l.product_code
WHERE i.customer_id = 10011
ORDER BY i.invoice_id, l.line_no;

SELECT 'SUMMARY' AS Label, SUM(l.quantity) AS TotalUnits, SUM(l.quantity * l.unit_price) AS TotalAmount
FROM Invoice i JOIN Line l ON l.invoice_id = i.invoice_id
WHERE i.customer_id = 10011;
