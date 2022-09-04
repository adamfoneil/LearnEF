I'm teaching myself EF Core because.... why not? I have a whole ecosystem of tools that use with a Dapper-based ORM approach, namely [ModelSync.UI](https://github.com/adamfoneil/ModelSync.UI). This is a rapid, low-friction way to evolve a database model, but it's intended for a shared development database. It's not meant for offline dev databases.

I've messed with EF before so I'm not totally new to it, but there are a couple things about it I still find excruciating.

# The Good
I like that I could implement [Base Table](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF.Database/Conventions/BaseTable.cs) conventions, such as a standard `Id` property and some common date stamp columns. I like that it was easy to set these properties [during a save](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF/OrdersContext.cs#L28-L32).

<details>
  <summary>EF Example</summary>
  
  ```csharp
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    foreach (var row in ChangeTracker.Entries<BaseTable>())
    {
        if (row.Entity.Id == 0) row.Entity.DateCreated = DateTime.Now;
        if (row.Entity.Id != 0) row.Entity.DateModified = DateTime.Now;
    }

    return await base.SaveChangesAsync(cancellationToken);
}
```
</details>

For comparison, I do this in [Dapper.Repository](https://github.com/adamfoneil/Dapper.Repository) with a [BeforeSaveAsync](https://github.com/adamfoneil/Dapper.Repository/blob/master/Dapper.Repository/Repository_virtuals.cs#L57) virtual method.

<details>
   <summary>Dapper Example</summary>
   
   ```csharp
   protected override async Task BeforeSaveAsync(IDbConnection connection, SaveAction action, TModel model, IDbTransaction txn = null)
  {
      switch (action)
      {
          case SaveAction.Insert:
              model.CreatedBy = Context.User.UserName;
              model.DateCreated = Context.User.LocalTime;
              break;

          case SaveAction.Update:
              model.ModifiedBy = Context.User.UserName;
              model.DateModified = Context.User.LocalTime;
              break;
      }

      await Task.CompletedTask;
  }
  ```
</details>

The migration experience is also not terrible with a little practice, which is what I needed. But in my opinion it's still not as smooth as ModelSync.

# The Bad
There's no declarative composite primary key syntax. You have to put composite keys in your `OnModelCreating` [override](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF/OrdersContext.cs#L19). I don't understand this at all. Anyone looking at this [Order model](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF.Database/Order.cs) won't know that it has a composite key. Composite keys have first-class support in ModelSync.

Along the same lines, foreign keys have a couple requirements and limitations I don't understand.

- There's no declaractive way to turn off cascade deletes. You have to put these in your migration such as [here](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF/Migrations/20220904174720_OrderTable.cs#L39) and [here](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF/Migrations/20220904174720_OrderTable.cs#L45). Again, ModelSync handles this with declarative syntax so anyone looking at the class at a glance whether it has cascade deletes or not.

- You have to add [collection](https://github.com/adamfoneil/LearnEF/blob/master/LearnEF.Database/Customer.cs#L12) properties on the primary side to create FKs, otherwise they're ignored. Why doesn't a `[ForeignKey]` attribute alone suffice?

<details>
  <summary>ModelSync Example</summary>
  
```csharp
public class Order : BaseTable
{
    [Key]
    [References(typeof(Customer), CascadeDelete = false)]
    public int CustomerId { get; set; }

    [Key]
    [References(typeof(Product), CascadeDelete = false)]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public decimal ExtPrice => Quantity * UnitPrice;
}
```
ModelSync has no trouble seeing this as
```sql
CREATE TABLE [dbo].[Order] (
    [Id] int identity(1,1)  NOT NULL,
    [CustomerId] int   NOT NULL,
    [ProductId] int   NOT NULL,
    [Quantity] int   NOT NULL,
    [UnitPrice] money   NOT NULL,
    [DateCreated] datetime   NOT NULL,
    [DateModified] datetime   NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY ([CustomerId] ASC, [ProductId] ASC),
    CONSTRAINT [U_Order_Id] UNIQUE ([Id] ASC)
)

GO

ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id])

GO

ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
```
</details>


