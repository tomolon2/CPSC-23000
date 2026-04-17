SELECT product_code AS ProductCode, description AS ProductDescription, last_received AS ProductInDate, on_hand AS QOH, reorder_point AS ProductMinimum
FROM Product
WHERE COALESCE(on_hand,0) <= COALESCE(reorder_point,0) + 5
ORDER BY product_code;
