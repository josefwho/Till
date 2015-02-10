<!DOCTYPE html>
<html>
<body>

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
//
// $regular = array();
// $overtime = array();
$totalProfit = array();
$totalItems = array();
		
	if ($result->num_rows > 0) 
{
	// output data of each row
	$i = 0;
	while($row = $result->fetch_assoc()) 
	{
		// $dateArray = explode("-",$row["Date"]);
		//
		// echo "exploded" . "<br>";
		// $arrlength = count($dateArray);
		// for ($j=0; $j < $arrlength; $j++) {
		// 	echo $dateArray[$j];
		// 	echo "<br>";
		// }
		//
		// $dateFn = "Date.UTC(" . $dateArray[0] . "," . $dateArray[1] . "," . $dateArray[2] . ")";
		// echo $dateFn . "<br>";
		// $regular[$i] = "[" . $dateFn . "," . $row["SummedRegular"] . "]";
		
		echo strtotime($row["Date"]);
		echo "<br>";
		echo date("Y.m.d", strtotime($row["Date"]));
		echo "<br>";
		
		// $regular[$row["Date"]] = $row["Summedegular"];
		$totalProfit[$row["Date"]] = $row["SummedProfit"];
		$totalItems[$row["Date"]] = $row["SummedRegular"] + $row["SummedOvertime"];
		
		$i++;
	}
} 
else
	echo "0 results";

$conn->close();

echo "all items sold during regular:";
echo "<br>";

// $arrlength = count($regular);
// for ($i=0; $i < $arrlength; $i++) {
// 	echo $regular[$i];
// 	echo "<br>";
// }

// foreach ($regular as $key => $value) {
// 	echo $key . ": " . $value;
// 	echo "<br>";
// }

// echo "all items sold during overtime:";
// echo "<br>";
// foreach ($overtime as $key => $value) {
// 	echo $key . ": " . $value;
// 	echo "<br>";
// }
echo "all items sold in total:";
echo "<br>";
foreach ($totalItems as $key => $value) {
	echo $key . ": " . $value;
	echo "<br>";
}		


echo "profit:";
echo "<br>";
$currentProfit = 0;
$profitToDraw = array();
$lastDate = null;
foreach ($totalProfit as $key => $value) 
{
	
	echo $key . ": " . $value;
	echo "<br>";
	
	$currentProfit += $value;

	if($lastDate != null)
	{
		$daysSinceLastProfit = ceil((strtotime($key) - strtotime($lastDate))/60/60/24);
		echo "dSLP: " . $daysSinceLastProfit . "<br>";
		$currentProfit -= $daysSinceLastProfit * (50.0 + mt_rand(-1000,1000)/1000 * 5);
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
	echo "is: " . count($profitToDraw) . "<br>";
}

$arrayLength = count($profitToDraw);
for ($i=0; $i < $arrayLength; $i++) { 
	echo $i . ": " . $profitToDraw[$i] . "<br>";
}

echo join($profitToDraw, ',');

		
// $output = "42"; //Again, do some operation, get the output.
// echo htmlspecialchars($output); /* You have to escape because the result will not be valid HTML otherwise. */
?>

</body>
</html>