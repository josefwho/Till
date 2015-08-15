<!DOCTYPE html>
<html>
<head>
	<title>Page Title</title>
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
	<script src="http://code.highcharts.com/highcharts.js"></script>
</head>

<body>

	<h1>This is a Heading</h1>

	<p>This is a paragraph.</p>
  
	<div id="dom-target" style="display: none;">
		<?php
		
		$servername = "wp338.webpack.hosteurope.de";
		$username = "db12273406-chest";
		$password = "lum?OTSn0us";
		$dbname = "db12273406-chesto";

		// Create connection
		$conn = new mysqli($servername, $username, $password, $dbname);
		// Check connection
		if ($conn->connect_error) 
		{
			die("Connection failed: " . $conn->connect_error);
		} 

		$sql = "SELECT Date,SUM(ItemsRegular) AS SummedRegular,SUM(ItemsOvertime) AS SummedOvertime,SUM(Profit) AS SummedProfit FROM `Sales`
			GROUP BY Date";
		
		$result = $conn->query($sql);
		
		$totalProfit = array();
		$totalItems = array();
		
		if ($result->num_rows > 0) 
		{
			// output data of each row
			$i = 0;
			while($row = $result->fetch_assoc()) 
			{
		
				$totalProfit[$row["Date"]] = $row["SummedProfit"];
				$totalItems[$row["Date"]] = $row["SummedRegular"] + $row["SummedOvertime"];
		
				$i++;
			}
		} 
		else
			echo "0 results";

		$conn->close();
		
		//calculate profit
		$currentProfit = 0;
		$profitToDraw = array();
		$lastDate = null;
		foreach ($totalProfit as $key => $value) 
		{
			$currentProfit += $value;

			if($lastDate != null)
			{
				$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
				$currentProfit -= $daysSinceLastProfit * (100.0 + mt_rand(-1000,1000)/1000 * 5);
			}

			$lastDate = $key;
	
			if(strtotime($key) > strtotime("-30 days"))
			{
				array_push($profitToDraw, $currentProfit);
			}
		}		

		//insert 0 profit if we don't have enough data yet
		$arrayLength = count($profitToDraw);
		if($arrayLength < 30)
		{
			echo "padding array. was: " . $arrayLength . "<br>";
			$profitToDraw = array_pad($profitToDraw, -30, 0);
			echo "is: " . count($profitToDraw);
		}
		
		//calculate day where graph is starting
		$tempDate = strtotime("-1 month"); //in highchart january is month 0 :)
		$startDate = date("Y,m,d", strtotime("-". count($profitToDraw) . " days", $tempDate));

		?>
	</div>
  
	<div id="container" style="width:400px; height:400px;"></div>
<script>
	$(function () {
		var options = {
        chart: {
            // type: 'bar'
        },
        title: {
            text: 'Fruit Consumption'
        },
        xAxis: {
			  type: 'datetime',
        },
        yAxis: {
            title: {
                text: 'Company Wealth in $'
            }
        },
		  plotOptions: {
           point: {
               radius: 1
           }
		  },
        series: [{
            name: 'Chesto',
			  	pointInterval: 24 * 3600 * 1000,
            pointStart: Date.UTC(<?php echo $startDate; ?>),
            data: [<?php echo join($profitToDraw, ','); ?>]
        }]
		};
	    $('#container').highcharts(
			 options
	    );
	});
</script>
</body>

</html>