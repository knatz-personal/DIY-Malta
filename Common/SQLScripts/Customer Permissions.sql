print 'ASSIGN TABLE PERMISSIONS'

GRANT SELECT ON "dbo"."UserTypes" TO "Customer" 
GRANT SELECT ON "dbo"."ContactTypes" TO "Customer" 
GRANT SELECT ON "dbo"."Contacts" TO "Customer" 
GRANT SELECT ON "dbo"."Genders" TO "Customer" 
GRANT SELECT ON "dbo"."Towns" TO "Customer" 
GRANT SELECT ON "dbo"."AddressTypes" TO "Customer" 
GRANT SELECT ON "dbo"."Addresses" TO "Customer" 
GRANT SELECT ON "dbo"."Roles" TO "Customer" 
GRANT SELECT ON "dbo"."UserRoles" TO "Customer" 
GRANT SELECT ON "dbo"."Menus" TO "Customer" 
GRANT SELECT ON "dbo"."MenuRoles" TO "Customer" 
GRANT SELECT ON "dbo"."Categorys" TO "Customer" 
GRANT SELECT ON "dbo"."SpecialSales" TO "Customer" 
GRANT SELECT ON "dbo"."Products" TO "Customer" 
GRANT SELECT ON "dbo"."OrderStates" TO "Customer" 
GRANT SELECT ON "dbo"."Orders" TO "Customer" 
GRANT SELECT ON "dbo"."OrderDetails" TO "Customer" 
GRANT SELECT ON "dbo"."ShoppingCarts" TO "Customer" 
GRANT SELECT ON "dbo"."PriceTypes" TO "Customer" 
GRANT SELECT ON "dbo"."Users" TO "Customer" 


GRANT UPDATE ON "dbo"."ShoppingCarts" TO "Customer" 
GRANT UPDATE ON "dbo"."Products" TO "Customer" 


GRANT INSERT ON "dbo"."Contacts" TO "Customer" 
GRANT INSERT ON "dbo"."Addresses" TO "Customer" 
GRANT INSERT ON "dbo"."UserRoles" TO "Customer" 
GRANT INSERT ON "dbo"."Orders" TO "Customer" 
GRANT INSERT ON "dbo"."OrderDetails" TO "Customer" 
GRANT INSERT ON "dbo"."ShoppingCarts" TO "Customer" 
GRANT INSERT ON "dbo"."Users" TO "Customer" 

GRANT DELETE ON "dbo"."ShoppingCarts" TO "Customer" 

print 'DONE'