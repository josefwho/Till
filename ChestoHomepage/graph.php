<!DOCTYPE html>
<html>
	<head>
		<link type="text/css" rel="stylesheet" href="stylesheet.css"/>
		<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
		<script src="http://code.highcharts.com/highcharts.js"></script>
		<title>Chesto Supermarkets Graph</title>
		<!-- <meta http-equiv="refresh" content="5" > -->
	</head>
	<body>
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
			$totalItems = 0;
			$lastDayItems = 0;
		
			if ($result->num_rows > 0) 
			{
				// output data of each row
				$i = 0;
				while($row = $result->fetch_assoc()) 
				{
		
					$totalProfit[$row["Date"]] = $row["SummedProfit"];
					$totalItems += $row["SummedRegular"] + $row["SummedOvertime"];
					
					$lastDayItems = $row["SummedRegular"] + $row["SummedOvertime"];
		
					$i++;
				}
			} 
			else
				echo "0 results";
		
			//calculate profit
			$currentProfit = 0;
			$profitToDraw = array();
			$lastDate = null;
			$lastProfit = 200;
			foreach ($totalProfit as $key => $value) 
			{

				//these are the values we will draw
				if(strtotime($key) > strtotime("-30 days"))
				{
					if($lastDate != null)
					{
						$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
						//reduce some money on first days NOT played
						// for ($i=0; $i < $daysSinceLastProfit-1; $i++)
						// {
						// 	$currentProfit -= $lastProfit * (0.3 - min($i*0.15, 0.3));
						// 	array_push($profitToDraw, $currentProfit);
						// }
						//finally add value on the day soneone played again
						$currentProfit = $currentProfit + $value;// - $lastProfit * (0.6 - min($daysSinceLastProfit*0.1, 0.4));
						array_push($profitToDraw, $currentProfit);
					}
					//first day
					else
					{
						$currentProfit += $value;
						array_push($profitToDraw, $currentProfit);
					}
				}
				//this is old data. we need it so profit is correct but we dont draw it
				else
				{
					$currentProfit += $value;
					
					//reduce some money, so graph can also go down a bit
					// if($lastDate != null)
					// {
					// 	$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
					// 	$currentProfit -= $daysSinceLastProfit * ( $lastProfit * 0.6);
					// }
					// echo $key . ": " . $value . " -> " . $currentProfit . "<br>";
				}

				$lastDate = $key;
				$lastProfit = $value;
			}		

			//insert 0 profit if we don't have enough data yet
			$arrayLength = count($profitToDraw);
			if($arrayLength < 30)
			{
				echo "padding array. was: " . $arrayLength . "<br>";
				$profitToDraw = array_pad($profitToDraw, -30, 0);
				echo "is: " . count($profitToDraw). "<br>";
			}
		
			//calculate day where graph is starting
			$tempDate = strtotime("-". count($profitToDraw)+1 . " days"); 
			$tempDate = strtotime("-1 month", $tempDate); //in highchart january is month 0 :)
			$startDate = date("Y,m,d", $tempDate);
			echo "tD: " . $tempDate . " sD: " . $startDate;

			$sql = "SELECT COUNT(*) AS Employees FROM `Sales` WHERE Date > DATE_SUB(CURDATE(),INTERVAL 1 MONTH) ";
			$result = $conn->query($sql);	
			
			$numberOfEmployees = 0;
			if ($result->num_rows > 0) 
			{
				// output data of each row
				$i = 0;
				while($row = $result->fetch_assoc()) 
				{
					$numberOfEmployees = $row["Employees"];
				}
			}  
			else
				echo "0 results";

			$sql = "SELECT * FROM `Sales` ORDER BY Date DESC, Time DESC LIMIT 1";
			$result = $conn->query($sql);	
			
			$lastProfit = 0;
			$lastItemsSold = 0;
			if ($result->num_rows > 0) 
			{
				// output data of each row
				$i = 0;
				while($row = $result->fetch_assoc()) 
				{
					$lastProfit = $row["Profit"];
					if($lastProfit < 0)
						$lastProfit = "- " . number_format(abs($lastProfit),2);
					else
						$lastProfit = "+ " . number_format($lastProfit,2);
					$lastItemsSold = (int)$row["ItemsRegular"] + (int)$row["ItemsOvertime"];
				}
			}  
			else
				echo "0 results";
			
			$conn->close();
			?>
		</div>

			<div style="background-color:white; height: 400px;" class ="clearfix">
				<h1 style="color:white"> INVESTORS</h1>

					<div id="graphContainerSolo" >
					<div>
						<script>
						$(function () {
							var options = {
								chart: {
									// type: 'bar'
								},
								title: {
									text: 'PBIT'
								},
								xAxis: {
									type: 'datetime',
								},
								yAxis: {
									title: {
										text: 'PBIT in EUR',
									}
								},
								tooltip: {
				                valueDecimals: 2,
				                valueSuffix: ' EUR',
								},
								plotOptions: {
									series: {
										marker: {
											enabled: false,
											states: {
												hover: {
													fillColor:'#57008e',
												}
											}
										},
										lineColor: '#57008e'
									}
								},
								series: [{
									name: 'Chesto',
									color: '#57008e',
									pointInterval: 24 * 3600 * 1000,
									pointStart: Date.UTC(<?php echo $startDate; ?>),
									data: [<?php echo join($profitToDraw, ','); ?>]
								}]
							};
							$('#graphContainerSolo').highcharts(
								options
							);
						});
						</script>
					</div>
				</div>
				
			</div>
		

<!-- 			<div style="background-color:white; height:150px; width=100%">
				<img id="left" style="height:auto; width:auto; max-height:130px; padding-top:10px; padding-left:3px"src="ShoppingItems/carrot.png">
				<img id="left" style="height:auto; width:auto; max-height:130px; padding-top:10px; padding-left:3px"src="ShoppingItems/carrot.png">
				<img id="left" style="height:auto; width:auto; max-height:130px; padding-top:10px; padding-left:3px"src="ShoppingItems/carrot.png">
				<img id="left" style="height:auto; width:auto; max-height:130px; padding-top:10px; padding-left:3px"src="ShoppingItems/carrot.png">
				<img id="left" style="height:auto; width:auto; max-height:130px; padding-top:10px; padding-left:3px"src="ShoppingItems/carrot.png">

		</div>
 -->
	</body>
</html>