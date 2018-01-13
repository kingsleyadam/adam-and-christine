<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trip.aspx.cs" Inherits="Adam_and_Christine_C.trip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Adam & Christine - Honeymoon</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="/css/trip.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="masthead clearfix">
            <div class="inner">
                <h3 class="masthead-brand">Adam & Christine
                </h3>
                <nav>
                    <ul class="nav masthead-nav">
                        <li class="active">
                            <a href="#">South Africa</a>
                        </li>
                        <li>
                            <a href="#">The Plan</a>
                        </li>
                    </ul>
                </nav>
            </div>
        </div>
        <div class="jumbotron">
            <div class="container">
                <h1>Adam & Christine</h1>
                <p>
                    Some text around us and our travels. What we like.
                </p>
            </div>
        </div>

        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <img src="../img/trip/south_africa.png" class="img-responsive" style="margin-left:auto;margin-right:auto;" alt="South Africa" />
                    <h2>South Africa</h2>
                    <p>Extra text to put here.</p>
                </div>
            </div>
            <hr class="featurette-divider" />
            <div class="row featurette">
                <div class="col-md-7">
                    <h2 class="featurette-heading">Enjoy <span class="text-muted">Cape Town</span>
                    </h2>
                    <p class="lead">Some information about Cape Town.</p>
                </div>
                <div class="col-md-5">
                    <img class="featurette-image img-responsive center-block img-rounded" style="margin-bottom: 5px;" alt="Cape Town" src="../img/trip/cape_town_1.png" data-holder-rendered="true" />
                    <img class="featurette-image img-responsive center-block img-rounded" alt="Cape Town" src="../img/trip/cape_town_2.png" data-holder-rendered="true" />
                </div>
            </div>
            <hr class="featurette-divider" />
            <div class="row featurette">
                <div class="col-md-7 col-md-push-5">
                    <h2 class="featurette-heading">Discovering <span class="text-muted">The WildLife</span>

                    </h2>
                    <p class="lead">Information about the animals</p>
                </div>
                <div class="col-md-5 col-md-pull-7">
                    <img class="featurette-image img-responsive center-block img-rounded" style="margin-bottom: 5px;" alt="Kruger Park" src="../img/trip/kruger_1.png" data-holder-rendered="true" />
                    <img class="featurette-image img-responsive center-block img-rounded" alt="Kruger Park" src="../img/trip/kruger_2.png" data-holder-rendered="true" />
                </div>
            </div>
            <hr class="featurette-divider" />
            <div class="row featurette">
                <div class="col-md-7">
                    <h2 class="featurette-heading">Relaxing <span class="text-muted">On The Beach</span>
                    </h2>
                    <p class="lead">Some information about Mauritius</p>
                </div>
                <div class="col-md-5">
                    <img class="featurette-image img-responsive center-block img-rounded" style="margin-bottom: 5px;" alt="Mauritius" src="../img/trip/mauritius_1.png" data-holder-rendered="true" />
                    <img class="featurette-image img-responsive center-block img-rounded" alt="Mauritius" src="../img/trip/mauritius_2.png" data-holder-rendered="true" />
                </div>
            </div>
            <hr class="featurette-divider" />
            <footer class="footer">
                <p class="pull-right">Back To Top</p>
                <p>
                    Adam & Christine - 6/6/2015
                </p>
            </footer>
        </div>


    </form>
    <script src="/js/jquery-2.1.1.min.js"></script>
    <script src="/js/bootstrap.min.js"></script>
</body>
</html>
