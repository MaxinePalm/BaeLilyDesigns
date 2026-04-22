# Bae Lily Designs 🧵

A web store for ordering organic hoodies designed by Brooklyn. Built with ASP.NET Core MVC on .NET 10, the site lets customers browse a curated collection of ethically-made hoodies, view product details, and add items to a session-based shopping cart.

---

## Features

- **Shop** - Browse all hoodies or filter by aesthetic category (Office, Goth, BookTok, Bright, Kids, Bubbly). Each product card shows name, emoji, price, badge (e.g. "Top Seller", "On Sale"), and availability.
- **Product Detail** - Full description, material info, available colours, sizes, and stock status. Customers can select a size and add to bag.
- **Sales Page** - Dedicated view listing all discounted items with original and sale prices.
- **Shopping Cart** - Session-backed cart with add, remove, and quantity update actions. A slide-out cart summary is accessible site-wide. Checkout clears the cart and confirms the pre-order.
- **About** - Brand story and key stats (supply chain transparency, aesthetic range, no compromises).
- **Ethics & Sustainability** - Detailed breakdown of environmental and ethical commitments: 70% organic cotton / 30% recycled polyester, fair wages, audited factories, recycled packaging, carbon-offset shipping, and size inclusivity (XS-3XL at a flat price).

---

## Project Structure

```
BaeLilyDesigns/
├── Controllers/
│   ├── HomeController.cs       # Landing page - shows bestsellers
│   ├── ShopController.cs       # Product listing and detail views
│   ├── CartController.cs       # Add / remove / update / checkout (JSON API)
│   ├── SalesController.cs      # Sale items listing
│   ├── AboutController.cs      # About page
│   └── SustainController.cs    # Ethics & sustainability page
├── Models/
│   ├── Product.cs              # Product entity (id, name, category, price, colours, sizes, stock, badge)
│   ├── ProductRepository.cs    # In-memory product catalogue with filtering helpers
│   ├── CartItem.cs             # Cart line item (product id, name, emoji, price, qty, size, colour)
│   └── ErrorViewModel.cs       # Error view support model
├── Views/
│   ├── Home/Index.cshtml
│   ├── Shop/{Index,Detail}.cshtml
│   ├── Sales/Index.cshtml
│   ├── About/Index.cshtml
│   ├── Sustain/Index.cshtml
│   └── Shared/{_Layout,_ProductCard,Error}.cshtml
├── wwwroot/
│   ├── css/site.css            # Custom styles
│   ├── js/site.js              # Cart interactions and UI behaviour
│   └── lib/                   # Bootstrap 5, jQuery, jQuery Validation
├── appsettings.json
├── Program.cs                  # App bootstrap and middleware pipeline
└── BaeLilyDesigns.csproj       # .NET 10 web project
```

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# |
| Front-end | Razor views, Bootstrap 5, jQuery |
| State | Server-side session (in-memory distributed cache) |
| Data | In-memory product repository (no database) |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run locally

```bash
git clone <repo-url>
cd BaeLilyDesigns/BaeLilyDesigns
dotnet run
```

The app will start on `https://localhost:5001` (or the port shown in the terminal). Open that URL in your browser.

### Build

```bash
dotnet build
```

---

## Product Catalogue

The store currently carries eight hoodies across six aesthetic categories:

| # | Name | Category | Price (R) | Badge |
|---|---|---|---|---|
| 1 | Pearl Party | Office | 1 100 | Top Seller |
| 2 | Midnight Bloom | Goth | 1 250 | - |
| 3 | Chapter One | BookTok | 1 100 | Fan Fave |
| 4 | Sunshine Hours | Bright | 950 ~~1 250~~ | On Sale |
| 5 | Cloud Nine | Kids | 750 | - |
| 6 | Soft Power | Bubbly | 1 100 | New |
| 7 | Gravestone Garden | Goth | 1 350 | - |
| 8 | The Annotator | BookTok | 1 200 ~~1 500~~ | On Sale |

All adult hoodies are available in XS-3XL at the same price. Kids' sizes run 3-4Y through 11-12Y. Every garment is made from 70% organic cotton and 30% recycled polyester.

---

## Cart API

The `CartController` exposes JSON endpoints consumed by the front-end JavaScript:

| Method | Route | Description |
|---|---|---|
| POST | `/Cart/Add` | Add a product (by id + size) to the session cart |
| POST | `/Cart/Remove` | Remove a product/size combination from the cart |
| POST | `/Cart/UpdateQty` | Increment or decrement quantity; removes item at 0 |
| GET | `/Cart/Summary` | Returns current items, total, and item count as JSON |
| POST | `/Cart/Checkout` | Clears the cart and confirms the pre-order |

Cart state is stored in an HTTP session with a 2-hour idle timeout.

---

## Sustainability Commitments

- ♻️ 100% recycled and biodegradable packaging - no single-use plastics
- 🏭 Independent ethical audits on all partner factories
- 🌱 Carbon-offset shipping through verified reforestation projects
- 👗 Pre-order model - no surplus, no waste
- 💛 XS–3XL size range at a flat price point
- 🔍 Full supply chain and cost transparency

---

## License

This project is the work of Brooklyn / Bae Lily Designs. All rights reserved.
