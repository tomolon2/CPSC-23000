SELECT p.product_code AS ProductCode, p.description AS ProductDescription, p.last_received AS ProductInDate, p.on_hand AS QOH
FROM Product p
LEFT JOIN Line l ON l.product_code = p.product_code
WHERE l.product_code IS NULL
ORDER BY p.description;
