﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class api_RetBonus : WOApiWebPage
{
    void GetInfo()
    {
        string CustomerID = web.CustomerID();

        SqlCommand sqcmd = new SqlCommand();
        sqcmd.CommandType = CommandType.StoredProcedure;
        sqcmd.CommandText = "WZ_RetBonusGetInfo";
        sqcmd.Parameters.AddWithValue("@in_CustomerID", CustomerID);

        if (!CallWOApi(sqcmd))
            return;

        StringBuilder xml = new StringBuilder();
        xml.Append("<?xml version=\"1.0\"?>\n");

        // current number of days
        reader.Read();
        xml.Append("<retbonus ");
        xml.Append(xml_attr("d", reader["RetDays"]));
        xml.Append(xml_attr("m", reader["MinsToNextDay"]));
        xml.Append(">");

        // report bonuses by day
        xml.Append("<days>");
        reader.NextResult();
        while (reader.Read())
        {
            xml.Append("<d ");
            xml.Append(xml_attr("b", reader["BonusItemID"]));
            xml.Append(xml_attr("q", reader["Quantity"]));
            xml.Append("/>");
        }
        xml.Append("</days>");
        xml.Append("</retbonus>");

        GResponse.Write(xml.ToString());
    }

    void GiveBonus()
    {
        string CustomerID = web.CustomerID();

        SqlCommand sqcmd = new SqlCommand();
        sqcmd.CommandType = CommandType.StoredProcedure;
        sqcmd.CommandText = "WZ_RetBonusGiveBonus";
        sqcmd.Parameters.AddWithValue("@in_CustomerID", CustomerID);

        if (!CallWOApi(sqcmd))
            return;

        reader.Read();
        Response.Write("WO_0");        
    }

    void CloseMessage()
    {
        string CustomerID = web.CustomerID();

        SqlCommand sqcmd = new SqlCommand();
        sqcmd.CommandType = CommandType.StoredProcedure;
        sqcmd.CommandText = "WZ_RetBonusCloseMessage";
        sqcmd.Parameters.AddWithValue("@in_CustomerID", CustomerID);
        sqcmd.Parameters.AddWithValue("@in_ActionID", web.Param("actionID"));

        if (!CallWOApi(sqcmd))
            return;

        reader.Read();
        Response.Write("WO_0");       
    }

    protected override void Execute()
    {
        if (!WoCheckLoginSession())
            return;

        string func = web.Param("func");
        if (func == "info")
            GetInfo();
        else if (func == "give")
            GiveBonus();
        else if (func == "close")
            CloseMessage();
        else
            throw new ApiExitException("bad func");
    }
}
