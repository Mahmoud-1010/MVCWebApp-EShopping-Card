**# E-commerce Application**

**Description**

This project is a full-stack e-commerce application built with ASP.NET Core MVC, leveraging Entity Framework for database interaction and Microsoft Azure for cloud deployment (optional). It offers a user-friendly platform for selling and managing products, with secure user authentication and a comprehensive admin dashboard.

**Features**

-   **Multi-Layered Architecture (NTier) with Repository Pattern and Unit of Work:**
    -   Ensures maintainability, code clarity, and efficient database interaction.
-   **Entity Framework:**
    -   Simplifies object-relational mapping for a productive development experience.
-   **Microsoft ASP Identity:**
    -   Provides robust user authentication and authorization for secure application access.
-   **Comprehensive Admin Dashboard:**
    -   Offers centralized application management, including product management, user management, and order processing.
-   **Enhanced User Experience:**
    -   Integrates jQuery Datatables for user-friendly data visualization and interaction with sorting, filtering, and pagination.
    -   Employs Toaster JS for non-intrusive user feedback.
-   **Pagination for Large Datasets:**
    -   Handles large datasets efficiently, preventing user interface overload.
-   **Stripe Integration:**
    -   Facilitates secure online payments for a smooth user experience.
-   **Microsoft Azure Cloud Deployment (Optional):**
    -   Scales the application for growth and provides reliable hosting (if applicable).

**Installation**

**Prerequisites:**

-   Visual Studio 2022 or later with ASP.NET and web development workload
-   Microsoft SQL Server (or a compatible database management system)
-   A Stripe account (for payment processing)
-   (Optional) Microsoft Azure account (for cloud deployment)

**Instructions:**

1.  Clone this repository:  `git clone https://github.com/Mahmoud-1010/MVCWebApp-EShopping-Card.git`
2.  Open the solution in Visual Studio.
3.  Update the `appsettings.json` file with your database connection string and Stripe API keys.
4.  (Optional) Configure Microsoft Azure deployment settings (if applicable).
5.  Build and run the application.

**Usage**

1.  **Run the Application:** Press `F5` in Visual Studio to start the application locally.
2.  **User Registration:** Create an account to access the platform.
3.  **Product Browsing:** Browse through available products.
4.  **Shopping Cart Management:** Add and remove items from your shopping cart.
5.  **Secure Checkout:** Proceed to checkout and securely complete your purchase using Stripe.
6.  **Admin Dashboard (if applicable):** Log in as an administrator to manage products, users, and orders.
