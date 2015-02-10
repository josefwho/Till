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
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 

for ($i=0; $i < 20; $i++) {
	$date = date("Y-m-d", strtotime("-". mt_rand($i-1, $i+1) . " days", strtotime("-20 days")));
	$sql = "INSERT INTO Sales
	VALUES ('". $date . "', CURTIME(), " . mt_rand(12, 42) . ", "  . mt_rand(2, 21) .  ", "  . mt_rand(55, 122) . "." . mt_rand(0, 122) . ")";

	echo $sql . "<br>";

	// if ($conn->query($sql) === TRUE) {
	//     echo "New record created successfully";
	// } else {
	//     echo "Error: " . $sql . "<br>" . $conn->error;
	// }
}



$conn->close();
?>

</body>
</html>