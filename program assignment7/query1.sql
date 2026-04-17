SELECT c.customer_id AS CustomerCode, c.first_name AS CustomerFirst, c.last_name AS CustomerLast, i.invoice_id AS InvoiceNumber,
       SUM(l.quantity * l.unit_price) AS InvoiceTotal
FROM Customer c
JOIN Invoice i ON i.customer_id = c.customer_id
JOIN Line l ON l.invoice_id = i.invoice_id
GROUP BY c.customer_id, c.first_name, c.last_name, i.invoice_id
ORDER BY c.customer_id, i.invoice_id;

SELECT 'GRAND TOTAL' AS Label, SUM(quantity * unit_price) AS Total FROM Line;
