SELECT vendor_id AS VendorCode, vendor_name AS VendorName, contact_last AS VendorContact, phone AS VendorPhone, state AS VendorState
FROM Vendor
WHERE area_code = '615' OR state = 'TN'
ORDER BY vendor_id;
