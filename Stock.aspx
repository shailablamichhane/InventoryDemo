<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stock.aspx.cs" Inherits="CellStockManagement.Stock" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="../assets/img/apple-icon.png">
    <link rel="icon" type="image/png" href="../assets/img/favicon.png">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Stock | CellStock</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://use.fontawesome.com/releases/v5.0.6/css/all.css" rel="stylesheet">
    <!-- CSS Files -->
    <link href="assets_forms/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets_forms/css/now-ui-dashboard.css?v=1.0.1" rel="stylesheet" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link href="../assets/demo/demo.css" rel="stylesheet" />
</head>
<body>
    <form runat="server">
        <div class="wrapper">
            <div class="sidebar" data-color="orange">
                <!--
        Tip 1: You can change the color of the sidebar using: data-color="blue | green | orange | red | yellow"
    -->
                <div class="logo">
                    <a href="Dashboard.aspx" class="simple-text logo-mini">CS
                    </a>
                    <a href="Dashboard.aspx" class="simple-text logo-normal">CellStock
                    </a>
                </div>
                <div class="sidebar-wrapper">
                    <ul class="nav">
                        <li>
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
                        <li class="active">
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
                        <% if (Session["userType"].ToString() == "Admin")
                           { %>
                        <li runat="server">
                            <a href="Users.aspx">
                                <i class="now-ui-icons users_circle-08"></i>
                                <p>ADD/LIST USERS</p>
                            </a>
                        </li>
                        <% } %>
                        <% if (Session["userType"].ToString() == "Staff")
                           { %>
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
                            <a class="navbar-brand" href="#pablo">Sales</a>
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
                                        <button class="btn btn-primary btn-block" onserverclick="btnLogout_Click" runat="server">
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
                <div class="panel-header panel-header-sm">
                </div>
                <div class="content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="title">Add Stock</h5>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4 pr-1">
                                            <div class="form-group" runat="server">
                                                <label>Product: </label>
                                                <select id="product_select" class="form-control" value="" runat="server">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server">
                                        <div class="col-md-4 pr-1">
                                            <div class="form-group" runat="server">
                                                <label>Quantity</label>
                                                <input id="product_quantity" runat="server" type="text" class="form-control" placeholder="Quantity" value="" onselect="getID" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="container" runat="server">
                                            <button id="addStockButton" runat="server" onserverclick="btn_AddStock_Click" class="btn btn-primary" type="submit">ADD</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="title">Search Products by Name</h5>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4 pr-1">
                                            <div class="form-group" runat="server">
                                                <label>Product Name</label>
                                                <select id="itemListForSearch" class="form-control" value="" runat="server">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 pr-1" runat="server">
                                            <button onserverclick="btn_SearchByName_Click" class="btn btn-primary" runat="server">Search</button>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 pr-1">
                                            <label>Product ID</label>
                                            <asp:Label ID="prodIDLabel" runat="server" />
                                        </div>
                                        <div class="col-md-4 px-1">
                                            <label>Product Name</label>
                                            <asp:Label ID="prodNameLabel" runat="server" />
                                        </div>
                                        <div class="col-md-4 pl-1">
                                            <label>Stock ID</label>
                                            <asp:Label ID="stockIDLabel" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 pr-1">
                                            <label>Stock Date</label>
                                            <asp:Label ID="stockDateLabel" runat="server" />
                                        </div>
                                        <div class="col-md-4 px-1">
                                            <label>Initial Quantity</label>
                                            <asp:Label ID="stockInitialQuantityLabel" runat="server" />
                                        </div>
                                        <div class="col-md-4 pl-1">
                                            <label>Remaining Quantity</label>
                                            <asp:Label ID="stockRemainingQuantityLabel" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="card-title">All Stock Details</h4>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <%--<table class="table">
                                        <thead class=" text-primary">
                                            <th>
                                                ID
                                            </th>
                                            <th>
                                                Product
                                            </th>
                                            <th>
                                                Initial Quantity
                                            </th>
                                            <th>
                                                Remaining Quantity
                                            </th> 
                                        </thead>
                                        <tbody runat="server">
                                            <%=populateTable()%>
                                        </tbody>
                                    </table>--%>
                                        <%--<asp:Table CssClass="table" ID="stockTable" runat="server">
                                        <asp:TableHeaderRow TableSection="TableHeader" CssClass="text-primary">
                                            <asp:TableHeaderCell>
                                                ID
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell>
                                                Product
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell>
                                                Initial Quantity
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell>
                                                Remaining Quantity
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                    </asp:Table>--%>
                                        <div id="grdCharges" runat="server" style="width: 1015px; overflow: auto;">
                                            <asp:GridView ID="stockGridView" CssClass="table table-hover table-bordered table-condensed" runat="server" DataKeyNames="id" OnRowDeleting="removeRow" OnRowCancelingEdit="cancelEditingRow" OnRowEditing="editRow" OnRowUpdating="updateRow" AutoGenerateColumns="false" AutoGenerateDeleteButton="true" AutoGenerateEditButton="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox Enabled="false" Width="20px" ID="TextBox1" runat="server" Text='<%# Bind("id") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox Enabled="false" ID="TextBox2" runat="server" Text='<%# Bind("product_name") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("product_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Initial Quantity" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("initial_quantity") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("initial_quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remaining Quantity" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("remaining_quantity") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("remaining_quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stock Date" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("stock_date") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("stock_date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Managed By" HeaderStyle-CssClass="text-primary" HeaderStyle-Font-Size="1.25em" HeaderStyle-Font-Bold="false">
                                                        <EditItemTemplate>
                                                            <asp:TextBox Enabled="false" ID="TextBox6" runat="server" Text='<%# Bind("managed_by") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("managed_by") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <footer class="footer">
                        <div class="container-fluid">
                            <p>CellStock</p>
                        </div>
                    </footer>
                </div>
            </div>
        </div>
    </form>
</body>
<!--   Core JS Files   -->
<script src="assets_forms/js/core/jquery.min.js"></script>
<script src="assets_forms/js/core/popper.min.js"></script>
<script src="assets_forms/js/core/bootstrap.min.js"></script>
<script src="assets_forms/js/plugins/perfect-scrollbar.jquery.min.js"></script>
<!--  Google Maps Plugin    -->
<script src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE"></script>
<!-- Chart JS -->
<script src="assets_forms/js/plugins/chartjs.min.js"></script>
<!--  Notifications Plugin    -->
<script src="assets_forms/js/plugins/bootstrap-notify.js"></script>
<!-- Control Center for Now Ui Dashboard: parallax effects, scripts for the example pages etc -->
<script src="assets_forms/js/now-ui-dashboard.js?v=1.0.1"></script>
<!-- Now Ui Dashboard DEMO methods, don't include it in your project! -->
<script src="../assets/demo/demo.js"></script>
</html>
