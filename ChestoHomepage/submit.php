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

$sql = "INSERT INTO Sales
VALUES (CURDATE(), CURTIME(), " . $_POST['itemsRegular'] . ", " . $_POST['itemsOvertime'] . ", " . $_POST['profit'] . ")";

if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>

</body>
</html>