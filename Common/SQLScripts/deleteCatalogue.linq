<Query Kind="SQL">
  <Connection>
    <ID>a4f2ae37-5444-49d5-882b-7b263a8c63d4</ID>
    <Persist>true</Persist>
    <Server>As-KNATZ-US</Server>
    <Database>DIYMalta</Database>
  </Connection>
  <Reference Relative="Assignments\WIP\WAD\Application\DIYMaltaProject\Common\bin\Debug\Common.dll">C:\Users\Natha_000\Dropbox\Assignments\WIP\WAD\Application\DIYMaltaProject\Common\bin\Debug\Common.dll</Reference>
  <Reference Relative="Assignments\WIP\WAD\Application\DIYMaltaProject\DAL\bin\Debug\DAL.dll">C:\Users\Natha_000\Dropbox\Assignments\WIP\WAD\Application\DIYMaltaProject\DAL\bin\Debug\DAL.dll</Reference>
  <Namespace>DAL</Namespace>
  <Namespace>DAL.Repositories</Namespace>
  <Namespace>System</Namespace>
</Query>

print 'START'
Delete ShoppingCarts
Delete OrderDetails
Delete Orders
Delete PriceTypes
Delete Products
print 'END'