<?php
	function db_connect()
	{
		$db_user   = "sa";
		$db_pass   = "!7ERX6VFbvB8#msyjFJJnJkkFdGx@!";
		$db_dbname = "zombiehunters";

    $db_serverName     = "192.168.100.13,1543";
		$db_connectionInfo = array(
			"UID" => $db_user,
			"PWD" => $db_pass,
			"Database" => $db_dbname,
			"CharacterSet" => "UTF-8"
			//"ReturnDatesAsStrings" => true
			);
		$conn = sqlsrv_connect($db_serverName, $db_connectionInfo);

		if(! $conn)
		{
			//echo "Connection could not be established.\n";
			die( print_r( sqlsrv_errors(), true));
			exit();
		}

		return $conn;
	}
	
	function db_exec($conn, $tsql, $params)
	{
		$stmt  = sqlsrv_query($conn, $tsql, $params);
		if(! $stmt)
		{
			echo "exec failed.\n";
			die( print_r( sqlsrv_errors(), true));
		}

		$member = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
		return $member;
	}

	$conn = db_connect();
?>