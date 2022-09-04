using LearnEF.Database;
using LearnEF.Services;
using Microsoft.EntityFrameworkCore;

using var db = new OrdersContext();

/*
db.Customers.AddRange(new[]
{
    new Customer() { Name = "Anyone" }, // renamed to Blitharius below
    new Customer() { Name = "Fishburne" },
    new Customer() { Name = "Wellington" }
});

await db.SaveChangesAsync();
*/

var firstRow = await db.Customers.Where(row => row.Id == 1).SingleAsync();
firstRow.Name = "Blitharius";
await db.SaveChangesAsync();

var customers = await db.Customers.ToListAsync();

foreach (var c in customers)
{
    Console.WriteLine($"{c.Name}, Id = {c.Id}, DateCreated = {c.DateCreated}, Modified = {c.DateModified}");
}
