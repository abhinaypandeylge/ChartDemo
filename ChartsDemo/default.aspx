<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ChartsDemo._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="scripts/jquery-3.5.1.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Chart.min.js"></script>
    <script>
        var projects=[];
        var openCount=[];
        var fixedCount=[];
        var inprogressDuein=[];
        var inprogressDueout=[];
        function getData () {
            $.ajax({
                type: "POST",
                url: "default.aspx/LoadData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    data = response.d;
                    for (var i = 0; i < data.length; i++) {
                        projects.push(data[i].Project);
                        openCount.push(data[i].CountOpen);
                        fixedCount.push(data[i].CountFixed);
                        inprogressDuein.push(data[i].CountInprogressDueIn);
                        inprogressDueout.push(data[i].CountInprogressDueout);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        $(function () {
            var response = getData();
            var chart = new Chart(ctx, {
                type: 'horizontalBar',                
                  data:{
                        labels: projects,
                        datasets: [{
                            label: 'Open',
                            stack: 0,
                            data: openCount,
                            backgroundColor: "rgba(217,242,27,1)",
                        },{
                            label: 'Inprogress(Above Due Date)',
                            stack: 1,
                            data: inprogressDueout,
                            backgroundColor: "rgba(242,6,2,1)",
                        }, {
                            label: 'Inprogress(Within Due Date)',
                            stack: 1,
                            data: inprogressDuein,
                            backgroundColor: "rgba(17,240,203,1)",
                        }, {
                            label: 'Fixed',
                            stack: 2,
                            data: fixedCount,
                            backgroundColor: "rgba(71,252,5,1)",
                        }]
                    },
                options: {
                    responsive: false,
                    legend: {
                        position: 'right'
                    },
                    scales: {
                        xAxes: [{
                            stacked: true
                        }],
                        yAxes: [{
                            stacked: true
                        }]
                    }
                }
            });
        

           
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <canvas id="cty" width="700"></canvas>
        
        <canvas id="ctx" width="700"></canvas>

    </form>
</body>
</html>
