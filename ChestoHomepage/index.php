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
			$lastProfit = 200;
			foreach ($totalProfit as $key => $value) 
			{

				//these are the values we will draw
				if(strtotime($key) > strtotime("-30 days"))
				{
					if($lastDate != null)
					{
						$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
						//reduce some money on all days NOT played
						for ($i=0; $i < $daysSinceLastProfit-1; $i++) 
						{ 
							$currentProfit -= $lastProfit * (0.6 - min($i*0.1, 0.4));
							array_push($profitToDraw, $currentProfit);
						}
						//finally add value on the day soneone played again
						$currentProfit = $currentProfit + $value - $lastProfit * (0.6 - min($daysSinceLastProfit*0.1, 0.4));
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
					
					if($lastDate != null)
					{
						$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
						//reduce some money, so graph can also go down a bit
						$currentProfit -= $daysSinceLastProfit * ( $lastProfit * 0.6);
					}
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
		
			<div style="width:100%; background-color:white; height:110px">
				<img class="chestoLogo"; align = "left"; style="height:auto; width:auto; max-width:300px; max-height:300px;" src="ChestoLogo.png">
			</div>


<!-- 			<div style="width:100%" align="center">
				<img style="height:auto; width:auto; max-width:800px; max-height:800px" src="landingPicture.png">
			</div> -->

			<div style="background-color:rgb(87,0,142)" class ="clearfix">
				<h1 style="color:white"> WHO WE ARE</h1>

					<div id="left" style="width:50%">
							<p id= "floatingTextWhite"> <span>HISTORY: <br></span>CHESTO was etablished 1993 by Lu Schesiner and taken over by his son Toni Schesiner in 2013.</p>
							<p id= "floatingTextWhite"> <span>OPERATIONS: <br></span>CHESTO is the world leading name in food retail solutions.</p>
							<p id= "floatingTextWhite"> <span>CAREER: <br></span>CHESTO employs the best people, develops their competence, provides opportunity and inspires them to live 
our values.</p>
							<p id= "floatingTextWhite"> <span>INTEGRITY: <br></span>CHESTO can always be trusted to do the right thing.</p>


					</div>

					<div id ="right">
						<p id= "floatingTextWhite" style="text-align:center; color:red"><span>PBIT</span></p>
						<div class= "chartCircleWhite";>

							<div><p id= "textWithinCircle" style="text-align:center; color:white"><span><?php echo $lastProfit; ?>&euro;<br>just now</span></p></div>
						</div>
						<p style="color:red; text-align:center;" id="floatingTextWhite"><span><?php echo number_format($profitToDraw[count($profitToDraw)-1],2); ?>&euro;<br> IN TOTAL</span> </p>
						
					</div>
					<div id ="right">
						<p id= "floatingTextWhite" style="text-align:center; color:red"> <span>ITEMS SOLD</span></p>
						<div class= "chartCircleWhite";>
							<p id= "textWithinCircle" style="text-align:center; color:white"><span>+ <?php echo $lastItemsSold; ?><br>just now</span></p>
						</div>
						<p id="floatingTextWhite" style="text-align:center; color:red"><span><?php echo $lastDayItems ?><br>TODAY</span></p>
 					</div>
				
			</div>
		


			<div style="background-color: white" class="clearfix">
				<h1> INVESTORS</h1>
				<div id="left" style="align:center; padding-left:0px">
					
					<div id ="left">
						<p id= "floatingTextPurple" style="text-align:center"> <span>OUR OPERATIONS</span></p>
						<div class= "chartCirclePurple">
							<div><p id="textWithinCircle"><span>23</span> <br>Countries</p></div>
						</div>
					</div>
					<div id ="left">
						<p id= "floatingTextPurple" style="text-align:center"> <span>OUR EMPLOYEES </span></p>
						<div class= "chartCirclePurple";>
							<div><p id="textWithinCircle"><span><?php echo $numberOfEmployees ?></span> <br> Employees</p></div>
						</div>
					</div>

				</div>


				<div id="graphContainer" >
					<p id="left" style="color:red"><span>SHARE</span></p>
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
							$('#graphContainer').highcharts(
								options
							);
						});
						</script>
					</div>
				</div>
			</div>

		<div style="background-color:rgb(87,0,142)" class="clearfix">
			<h1 style="color:white"> NEWS</h1>
			<div id="left" style="width:600px; padding-left:0px">
				<a href="http://toniluschesiner.tumblr.com"><p id="left" style="color: white"> Meet our CEO <span>TONI SCHESINER</span> on his personal blog.</p></a>
				<a href="http://toniluschesiner.tumblr.com"><p id="left" style="color: white"> How does the life of one of the most <span>influential</span> men look like?</p></a>
				<a href="http://toniluschesiner.tumblr.com"><p id="left" style="color: white"> Do you want to have a peek inside the life of CHESTO's <span>main man</span>?</p></a>
				<a href="http://toniluschesiner.tumblr.com"><p id="left" style="color: white"> Visit: <span>http://toniluschesiner.tumblr.com</span></p>
   			</div>
<!-- 				<img id="left" style="height:auto; width:auto; max-width:180px; max-height:180px; padding-top:25px"src="ShoppingItems/soda.png">
				<img id="left" style="height:auto; width:auto; max-width:180px; max-height:180px; padding-top:25px"src="ShoppingItems/champagne.png">
 -->
			<div id= "right" style="width:10%">
				<a href="http://toniluschesiner.tumblr.com"><img id="right" style="height:auto; width:auto; max-width:280px; max-height:280px;"src="Toni.png"></a>
			</div>

		</div>

		<div style="background-color:white" class="clearfix">
			<h1> SHAREHOLDERS</h1>
				<div id="left" style="width:70%">
					<p id= "floatingTextBlack" style="color:rgb(87,0,142)"> <span>CHESTO</span> wants to thank its main shareholders <br><a href="http://www.brokenrul.es"><span>BROKEN RULES</span></a> & <a href="https://twitter.com/peregrinustyss"><span>JOSEF WHO</span></a></p>
				</div>
				<div id="right">
					<a id="left" href="http://www.brokenrul.es"><img style="height:auto; width:auto; max-width:80px; max-height:80px;"src="BRLogo.png"></a> 
					<a id="left" href="https://twitter.com/peregrinustyss"><img style="height:auto; width:auto; max-height:80px;"src="Josef.jpg"> </a>
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