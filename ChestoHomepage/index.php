<!DOCTYPE html>
<html>
	<head>
		<link type="text/css" rel="stylesheet" href="stylesheet.css"/>
		<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
		<script src="http://code.highcharts.com/highcharts.js"></script>
		<title>Chesto Supermarkets</title>
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
			foreach ($totalProfit as $key => $value) 
			{
				$currentProfit += $value;

				if($lastDate != null)
				{
					$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
					$currentProfit -= $daysSinceLastProfit * ( 100.0 + mt_rand(-1000,1000)/1000 * 5);
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

			$conn->close();
			?>
		</div>
		
			<div style="width:100%; background-color:white; height:110px">
				<img class="chestoLogo"; align = "left"; style="height:auto; width:auto; max-width:300px; max-height:300px;" src="ChestoLogo.png">
			</div>

<!-- 			<div style="width:100%" align="center">
				<img style="height:auto; width:auto; max-width:800px; max-height:800px" src="landingPicture.png">
			</div> -->

			<div style="background-color:rgb(87,0,142); height:350px">
				<h1 style="color:white"> WHO WE ARE</h1>

					<div id="left" style="width:40%">
							<p id= "floatingTextWhite"> <span>CHESTO</span> was etablished 1983 by Guenther Eisner and taken over by his son in 2013.</p>
							<p id= "floatingTextWhite"> <span>CHESTO</span> is one of the leading food retail companies in the world.</p>
					</div>
				<div id ="right">
					<div id ="right">
						<p id= "floatingTextWhite" style="text-align:center"> <span>OUR OPERATIONS</span></p>
						<div class= "chartCircleWhite">
							<div><p id="textWithinCircle"><span>69</span> countries</p></div>
						</div>
					</div>

					</div>
						<div id ="right">
						<p id= "floatingTextWhite" style="text-align:center"> <span>OUR EMPLOYEES </span></p>
								<div class= "chartCircleWhite";>
									<p id="textWithinCircle"><span><?php echo $numberOfEmployees ?></span> <br> Employees</p>
								</div>
					</div>
				</div>
			</div>
		


			<div style="background-color: white; height:350px">
				<h1> INVESTORS</h1>
				<div id="left" style="align:center">
					<div id ="left">
						<p id= "floatingTextBlack" style="text-align:center; color:red"><span>PBIT</span></p>
						<div class= "chartCirclePurple";>
							<div><p style="color:black" id="textWithinCircle"><span><?php echo number_format($profitToDraw[count($profitToDraw)-1],2) ?></span> <br> $</p></div>
						</div>
					</div>
					<div id ="left">
						<p id= "floatingTextBlack" style="text-align:center; color:red"> <span>ITEMS SOLD</span></p>
						<div class= "chartCirclePurple";>
							<p id="textWithinCircle"><span><?php echo $lastDayItems ?></span>  <br>today</p>
						</div>
					</div>
				</div>

					<div id="graphContainer" >
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
							                text: 'PBIT in $',
							            }
							        },
									  tooltip: {
										  pointFormat: '{point.y} $',
										  valueDecimals: 2
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
								    $('#graphContainer').highcharts(
										 options
								    );
								});
							</script>
						</div>
						<p id="left" style="color:red"><span>SHARE</span></p>
					</div>
			</div>

			<div style="background-color:rgb(87,0,142); height:350px">
				<h1 style="color:white"> NEWS</h1>
				<p id="left" style="color: white"> MEET OUR CEO <span>TONI EISNER</span> ON HIS PERSONAL BLOG</p>
				<img id="right" style="height:auto; width:auto; max-width:248px; max-height:248px;"src="Toni.png">
			</div>

			<div style="background-color:white";>
				<h1> SHAREHOLDERS</h1>
					<div id="left" style="width:40%">
							<p id= "floatingTextBlack" style="color:rgb(87,0,142)"> <span>CHESTO</span> wants to thank its main shareholders <span>BROKEN RULES</span> & <span>JOSEF WHO</span></p>
					</div>
					<div id="right">
						<a id="left" href="http://www.brokenrul.es"><img style="height:auto; width:auto; max-width:80px; max-height:80px;"src="BRLogo.png"></a> 
						<a id="left" href="https://twitter.com/peregrinustyss"><img style="height:auto; width:auto; max-height:80px;"src="Josef.jpg"> </a>
						</div>
			</div>

	</body>
</html>