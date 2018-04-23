using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.SessionState;
using System.ComponentModel;

public class OrderItemBase {
    IList owner;

    public OrderItemBase() {

    }
    public OrderItemBase(IList owner) {
        this.owner = owner;
    }
    protected IList Owner { get { return owner; } }
}

public class OrderProduct : OrderItemBase {
    int id;
    string name = string.Empty;
    decimal price = 0;

    public OrderProduct()
        : base() {
    }
    public OrderProduct(IList owner, int id)
        : base(owner) {
        this.id = id;
    }
    public int Id { get { return id; } set { id = value; } }
    public string Name { get { return name; } set { name = value; } }
    public decimal Price { get { return price; } set { price = value; } }
}

public class OrderItem : OrderItemBase {
    HttpSessionState Session { get { return HttpContext.Current.Session; } }

    int id;
    int productId;
    int quantity;
    IList products;
    OrderProduct product;

    public OrderItem()
        : base() {
        this.products = Session["OrderProducts"] as BindingList<OrderProduct>;
    }
    public OrderItem(IList owner, IList products, int id)
        : base(owner) {
        this.id = id;
        this.products = products;
        this.quantity = 1;
    }

    public int Id { get { return id; } set { id = value; } }
    public OrderProduct Product { get { return product; } }
    public int ProductId {
        get { return productId; }
        set {
            productId = value;
            SetProduct();
        }
    }
    public int Quantity { get { return quantity; } set { quantity = value; } }
    public decimal Price { get { return Product != null ? Product.Price : 0; } }
    public decimal Total { get { return Price * Quantity; } }
    void SetProduct() {
        this.product = null;
        foreach (OrderProduct item in this.products) {
            if (item.Id == ProductId) {
                this.product = item;
                break;
            }
        }
    }
}

public class OrderItemsProvider {
    HttpSessionState Session { get { return HttpContext.Current.Session; } }

    public BindingList<OrderProduct> GetProducts() {
        BindingList<OrderProduct> products = Session["OrderProducts"] as BindingList<OrderProduct>;
        if (products == null) {
            products = CreateProducts();
            Session["OrderProducts"] = products;
        }
        return products;
    }
    public BindingList<OrderItem> GetItems() {
        BindingList<OrderItem> items = Session["OrderItems"] as BindingList<OrderItem>;
        if (items == null) {
            items = CreateOrderItems();
            Session["OrderItems"] = items;
        }
        return items;
    }
    public void ItemDelete(int id) {
        OrderItem item = GetItemById(id);
        if (item != null) {
            GetItems().Remove(item);
        }
    }
    public void ItemUpdate(int productId, int quantity, int id) {
        OrderItem item = GetItemById(id);
        if (item != null) {
            ItemUpdateCore(item, productId, quantity);
        }
    }
    public void ItemInsert(int productId, int quantity) {
        OrderItem item = new OrderItem(GetItems(), GetProducts(), GetItems().Count);
        ItemUpdateCore(item, productId, quantity);
        GetItems().Add(item);
    }
    void ItemUpdateCore(OrderItem item, int productId, int quantity) {
        item.Quantity = quantity;
        item.ProductId = productId;
    }
    public void ProductUpdate(string name, decimal price, int id) {
        OrderProduct item = GetProductById(id);
        if (item != null) {
            ProductUpdateCore(item, name, price);
        }
    }
    public void ProductInsert(string name, decimal price) {
        OrderProduct item = new OrderProduct(GetProducts(), GetProducts().Count);
        ProductUpdateCore(item, name, price);
        GetProducts().Add(item);
    }
    void ProductUpdateCore(OrderProduct item, string name, decimal price) {
        item.Name = name;
        item.Price = price;
    }
    public OrderItem GetItemById(int id) {
        foreach (OrderItem item in GetItems()) {
            if (item.Id == id)
                return item;
        }
        return null;
    }
    public OrderProduct GetProductById(int id) {
        foreach (OrderProduct item in GetProducts()) {
            if (item.Id == id)
                return item;
        }
        return null;
    }
    BindingList<OrderProduct> CreateProducts() {
        BindingList<OrderProduct> res = new BindingList<OrderProduct>();
        Random r = new Random();
        for (int i = 0; i < 5; i++) {
            OrderProduct item = new OrderProduct(res, i + 1);
            item.Name = string.Format("Product {0}", i + 1);
            item.Price = r.Next(10, 200);
            res.Add(item);
        }
        return res;
    }
    BindingList<OrderItem> CreateOrderItems() {
        BindingList<OrderItem> res = new BindingList<OrderItem>();
        BindingList<OrderProduct> products = GetProducts();
        Random r = new Random();
        for (int i = 0; i < 7; i++) {
            OrderItem item = new OrderItem(res, products, i);
            item.ProductId = (i % products.Count) + 1;
            item.Quantity = r.Next(1, 100);
            res.Add(item);
        }
        return res;
    }
}





