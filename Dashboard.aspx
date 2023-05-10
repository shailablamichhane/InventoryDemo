<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CellStockManagement.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="../assets/img/apple-icon.png">
    <link rel="icon" type="image/png" href="../assets/img/favicon.png">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Dashboard | CellStock</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://use.fontawesome.com/releases/v5.0.6/css/all.css" rel="stylesheet">
    <!-- CSS Files -->
    <link href="assets_forms/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets_forms/css/now-ui-dashboard.css?v=1.0.1" rel="stylesheet" />
</head>
<body>
    <form runat="server">
    <div class="wrapper ">
        <div class="sidebar" data-color="orange">
            <!--
        Tip 1: You can change the color of the sidebar using: data-color="blue | green | orange | red | yellow"
    -->
            <div class="logo">
                <a href="#" class="simple-text logo-mini">CS
                </a>
                <a href="#" class="simple-text logo-normal">
                    CellStock
                </a>
            </div>
            <div class="sidebar-wrapper" runat="server">
                <ul class="nav" runat="server">
                    <li class="active">
                        <a href="Dashboard.aspx">
                            <i class="now-ui-icons business_chart-bar-32"></i>
                            <p>DASHBOARD</p>
                        </a>
                    </li>
                    <li>
                        <a href="Products.aspx">
                            <i class="now-ui-icons design_app"></i>
                            <p>ADD/LIST PRODUCTS</p>
                        </a>
                    </li>
                    <li>
                        <a href="Stock.aspx">
                            <i class="now-ui-icons shopping_box"></i>
                            <p>STOCK DETAILS</p>
                        </a>
                    </li>
                    <li>
                        <a href="Customers.aspx">
                            <i class="now-ui-icons users_single-02"></i>
                            <p>ADD/LIST CUSTOMERS</p>
                        </a>
                    </li>
                    <li>
                        <a href="Sales.aspx">
                            <i class="now-ui-icons files_single-copy-04"></i>
                            <p>ADD/LIST SALES</p>
                        </a>
                    </li>
                    <li>
                        <a href="Suppliers.aspx">
                            <i class="now-ui-icons transportation_bus-front-12"></i>
                            <p>ADD/LIST SUPPLIERS</p>
                        </a>
                    </li>
                    <% if (Session["userType"].ToString() == "Admin") { %>
                    <li runat="server">
                            <a href="Users.aspx">
                                <i class="now-ui-icons users_circle-08"></i>
                                <p>ADD/LIST USERS</p>
                            </a>
                    </li>
                    <% } %>

                    <% if (Session["userType"].ToString() == "Staff") { %>
                    <li runat="server">
                            <a href="ChangePassword.aspx">
                                <i class="now-ui-icons users_circle-08"></i>
                                <p>CHANGE PASSWORD</p>
                            </a>
                    </li>
                    <% } %>
                </ul>
            </div>
        </div>
        <div class="main-panel">
            <!-- Navbar -->
            <nav class="navbar navbar-expand-lg navbar-transparent  navbar-absolute bg-primary fixed-top">
                <div class="container-fluid">
                    <div class="navbar-wrapper">
                        <div class="navbar-toggle">
                            <button type="button" class="navbar-toggler">
                                <span class="navbar-toggler-bar bar1"></span>
                                <span class="navbar-toggler-bar bar2"></span>
                                <span class="navbar-toggler-bar bar3"></span>
                            </button>
                        </div>
                        <a class="navbar-brand" href="#pablo">Dashboard</a>
                    </div>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navigation" aria-controls="navigation-index" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-bar navbar-kebab"></span>
                        <span class="navbar-toggler-bar navbar-kebab"></span>
                        <span class="navbar-toggler-bar navbar-kebab"></span>
                    </button>
                    <div class="collapse navbar-collapse justify-content-end" id="navigation">
                        <ul class="navbar-nav">
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="Dashboard.aspx" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="now-ui-icons users_single-02"></i>
                                    <p>
                                        WELCOME, <%:Session["username"]%>!
                                        <span class="d-lg-none d-md-block"></span>
                                    </p>
                                </a>
                                <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownMenuLink" runat="server" onserverclick="btnLogout_Click">
                                    
                            <%--<asp:Button CssClass="btn btn-primary" ID="Button1" runat="server" Text="Logout" OnClick="Button1_Click" />--%>
                                            <button class="btn btn-primary btn-block" onServerClick="btnLogout_Click" runat="server">
                                         <i class="now-ui-icons ui-1_simple-remove"></i>
                                    <p>LOGOUT</p>
                                </button>
                                </div>

                            </li>
                        </ul>
                    </div>
                </div>
            </nav> 
            <!-- End Navbar -->
            <% if (Convert.ToBoolean(Session["lowOnStock"])) { %>
            <div class="alert alert-dismissible alert-danger notif" style="position: fixed; min-height: 200px; width: 300px; bottom: 20px; right: 20px; z-index:20">
                <button type="button" class="close" data-dismiss="alert">&times;</button>
                <h5>Products Low on Stock:</h5>
                <asp:Label runat="server" ID="lblAlert"><%=getItemsLowOnStock()%></asp:Label>
            </div>
            <% } %>
            <div class="panel-header panel-header-lg">
                
                <canvas id="bigDashboardChart"></canvas>
                
            </div>
            <div class="content">
                <div class="row">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="title text-center">Products Out Of Stock</h5>
                                <p class="text-muted text-center">Products that have 0 quantity left in stock.</p>
                            </div>
                            <div class="card-body">
                            	<div class="row">
                                    <div class="container">
                                    <table class="table table-responsive table-bordered">
                                        <thead class="text-primary">
                                            <th>Stock ID</th>
                                            <th>Product ID</th>
                                            <th>Product</th>
                                            <th class="text-right">Remaining Quantity</th>
                                        </thead>
                                        <tbody runat="server">
                                            <%=populateOutOfStockTable()%>
                                        </tbody>
                                    </table>
                                </div>
                            	</div>
                            </div>
                        </div>
                </div>
                <div class="row">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="title text-center">Inactive Customers</h5>
                                <p class="text-muted text-center">Customers who haven't bought any item in the past 30 days.</p>
                            </div>
                            <div class="card-body">
                            	<div class="row">
                                    <div class="container">
                                    <table class="table table-bordered table-responsive">
                                        <thead class="text-primary">
                                            <th>Customer ID</th>
                                            <th>Name</th>
                                            <th>Phone No.</th>
                                            <th class="text-right">Email</th>
                                        </thead>
                                        <tbody runat="server">
                                            <%=populateInactiveCustomerTable()%>
                                        </tbody>
                                    </table>
                                </div>
                            	</div>
                            </div>
                        </div>
                </div>
                <div class="row">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="title text-center">Unpopular Items</h5>
                                <p class="text-muted text-center">Items that haven't been sold in the last 100 days.</p>
                            </div>
                            <div class="card-body">
                            	<div class="row">
                                    <div class="container">
                                    <table class="table table-bordered table-responsive">
                                        <thead class="text-primary">
                                            <th>Product ID</th>
                                            <th>Product</th>
                                            <th class="text-right">Last Sales Date</th>
                                        </thead>
                                        <tbody runat="server">
                                            <%=populateUnpopularItemsTable()%>
                                        </tbody>
                                    </table>
                                        </div>
                                </div>
                            	</div>
                            </div>
                        </div>
                </div>
            </div>
            <footer class="footer">
            </footer>
        </div>
    </div>
        </form>
</body>
<!--   Core JS Files   -->
<script src="/assets_forms/js/core/jquery.min.js"></script>
<script src="/assets_forms/js/core/popper.min.js"></script>
<script src="/assets_forms/js/core/bootstrap.min.js"></script>
<script src="/assets_forms/js/plugins/perfect-scrollbar.jquery.min.js"></script>
<!-- Chart JS -->
<script src="/assets_forms/js/plugins/chartjs.min.js"></script>
<!--  Notifications Plugin    -->
<script src="/assets_forms/js/plugins/bootstrap-notify.js"></script>
<!-- Control Center for Now Ui Dashboard: parallax effects, scripts for the example pages etc -->
<script src="/assets_forms/js/now-ui-dashboard.js?v=1.0.1"></script>
    <script src="/assets_forms/demo/demo.js"></script>
<script>
    $(document).ready(function () {
        demo.initDashboardPageCharts();
    });

</script>
    <script>
        document.onload(function () {
            window.alert("An item is running low on stock!")
        })
    </script>

</html>
