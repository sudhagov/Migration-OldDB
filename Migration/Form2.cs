using Infragistics.Win.UltraMessageBox;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Migration
{
    public partial class Form2 : Form
    {
        public static UdpClient client = new UdpClient(4124);
        public static string DeltaTIPAddress = "";
        public static Byte SequenceNumber = 0;
        public static DataTable table1 = new DataTable("Gateways");
        public static DataTable table2 = new DataTable("Gateways");
        public static DataTable table3 = new DataTable("SensorData");
        public static byte GatewayCount = 0;
        public static string DeviceMAC { get; set; }
        delegate void Func();
        string Occupied = "No";
        byte bCurrentNodeID;
        public static string currentNodeID = "";

        public class ZWaveUDP
        {
            public byte CommandClass;
            public byte Command;
            public byte properties1;
            public byte properties2;
            public byte seqNumber;
            public byte SourceEndPoint;
            public byte DestinationEndPoint;
            public byte Header1;
            public byte Header2;
            public byte Header3;
            public byte Header4;
            public byte Header5;
            public byte ZWCommandClass;
            public byte ZWCommand;
            public byte ZWpayload1;
            public byte ZWpayload2;
            public byte ZWpayload3;
            public byte ZWpayload4;
            public byte ZWpayload5;
            public byte ZWpayload6;


        }

        static class ZWConst
        {
            public const byte ZIP_VERSION_V2 = 0x02;

            public const byte COMMAND_ZIP_PACKET_V2 = 0x02;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_RESERVED_MASK_V2 = 0x03;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_NACK_OPTION_ERROR_BIT_MASK_V2 = 0x04;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_NACK_QUEUE_FULL_BIT_MASK_V2 = 0x08;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_NACK_WAITING_BIT_MASK_V2 = 0x10;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_NACK_RESPONSE_BIT_MASK_V2 = 0x20;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_ACK_RESPONSE_BIT_MASK_V2 = 0x40;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES1_ACK_REQUEST_BIT_MASK_V2 = 0x80;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES2_RESERVED_MASK_V2 = 0x0F;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES2_SECURE_ORIGIN_BIT_MASK_V2 = 0x10;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES2_MORE_INFORMATION_BIT_MASK_V2 = 0x20;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES2_Z_WAVE_CMD_INCLUDED_BIT_MASK_V2 = 0x40;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES2_HEADER_EXT_INCLUDED_BIT_MASK_V2 = 0x80;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES3_SOURCE_END_POINT_MASK_V2 = 0x7F;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES3_RES_BIT_MASK_V2 = 0x80;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES4_DESTINATION_END_POINT_MASK_V2 = 0x7F;

            public const byte COMMAND_ZIP_PACKET_PROPERTIES4_BIT_ADDRESS_BIT_MASK_V2 = 0x80;

            public const byte COMMAND_CLASS_ALARM = 0x71;




            /* Network Management Proxy command class commands */

            public const byte NETWORK_MANAGEMENT_PROXY_VERSION = 0x01;

            public const byte NODE_INFO_CACHED_GET = 0x03;

            public const byte NODE_INFO_CACHED_REPORT = 0x04;

            public const byte NODE_LIST_GET = 0x01;

            public const byte NODE_LIST_REPORT = 0x02;

            /* Values used for Node Info Cached Get command */

            public const byte NODE_INFO_CACHED_GET_PROPERTIES1_MAX_AGE_MASK = 0x0F;

            public const byte NODE_INFO_CACHED_GET_PROPERTIES1_RESERVED_MASK = 0xF0;

            public const byte NODE_INFO_CACHED_GET_PROPERTIES1_RESERVED_SHIFT = 0x04;

            /* Values used for Node Info Cached Report command */

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES1_AGE_MASK = 0x0F;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES1_STATUS_MASK = 0xF0;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES1_STATUS_SHIFT = 0x04;

            public const byte NODE_INFO_CACHED_REPORT_STATUS_STATUS_OK = 0x00;

            public const byte NODE_INFO_CACHED_REPORT_STATUS_STATUS_NOT_RESPONDING = 0x01;

            public const byte NODE_INFO_CACHED_REPORT_STATUS_STATUS_UNKNOWN = 0x02;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES2_CAPABILITY_MASK = 0x7F;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES2_LISTENING_BIT_MASK = 0x80;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES3_SECURITY_MASK = 0x0F;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES3_SENSOR_MASK = 0x70;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES3_SENSOR_SHIFT = 0x04;

            public const byte NODE_INFO_CACHED_REPORT_PROPERTIES3_OPT_BIT_MASK = 0x80;

            public const byte NODE_INFO_CACHED_REPORT_SECURITY_SCHEME_0_MARK = 0x00;



            /* Network Management Basic command class commands */

            public const byte NETWORK_MANAGEMENT_BASIC_VERSION = 0x01;

            public const byte LEARN_MODE_SET = 0x01;

            public const byte LEARN_MODE_SET_STATUS = 0x02;

            public const byte NODE_INFORMATION_SEND = 0x05;

            public const byte NETWORK_UPDATE_REQUEST = 0x03;

            public const byte NETWORK_UPDATE_REQUEST_STATUS = 0x04;

            public const byte DEFAULT_SET = 0x06;

            public const byte DEFAULT_SET_COMPLETE = 0x07;



            /* Network Management Inclusion command class commands */

            public const byte NETWORK_MANAGEMENT_INCLUSION_VERSION = 0x01;

            public const byte FAILED_NODE_REMOVE = 0x07;

            public const byte FAILED_NODE_REMOVE_STATUS = 0x08;

            public const byte NODE_ADD = 0x01;

            public const byte NODE_ADD_STATUS = 0x02;

            public const byte NODE_REMOVE = 0x03;

            public const byte NODE_REMOVE_STATUS = 0x04;

            public const byte FAILED_NODE_REPLACE = 0x09;

            public const byte FAILED_NODE_REPLACE_STATUS = 0x0A;

            public const byte NODE_NEIGHBOR_UPDATE_REQUEST = 0x0B;

            public const byte NODE_NEIGHBOR_UPDATE_STATUS = 0x0C;

            public const byte RETURN_ROUTE_ASSIGN = 0x0D;

            public const byte RETURN_ROUTE_ASSIGN_COMPLETE = 0x0E;

            public const byte RETURN_ROUTE_DELETE = 0x0F;

            public const byte RETURN_ROUTE_DELETE_COMPLETE = 0x10;

            /* Values used for Node Add Status command */

            public const byte NODE_ADD_STATUS_PROPERTIES1_CAPABILITY_MASK = 0x7F;

            public const byte NODE_ADD_STATUS_PROPERTIES1_LISTENING_BIT_MASK = 0x80;

            public const byte NODE_ADD_STATUS_PROPERTIES2_SECURITY_MASK = 0x7F;

            public const byte NODE_ADD_STATUS_PROPERTIES2_OPT_BIT_MASK = 0x80;


            public const byte COMMAND_CLASS_ALARM_V2 = 0x71;

            public const byte COMMAND_CLASS_NOTIFICATION_V3 = 0x71;

            public const byte COMMAND_CLASS_NOTIFICATION_V4 = 0x71;

            public const byte COMMAND_CLASS_APPLICATION_STATUS = 0x22;

            public const byte COMMAND_CLASS_ASSOCIATION_COMMAND_CONFIGURATION = 0x9B;

            public const byte COMMAND_CLASS_ASSOCIATION = 0x85;

            public const byte COMMAND_CLASS_ASSOCIATION_V2 = 0x85;

            public const byte COMMAND_CLASS_AV_CONTENT_DIRECTORY_MD = 0x95;

            public const byte COMMAND_CLASS_AV_CONTENT_SEARCH_MD = 0x97;

            public const byte COMMAND_CLASS_AV_RENDERER_STATUS = 0x96;

            public const byte COMMAND_CLASS_AV_TAGGING_MD = 0x99;

            public const byte COMMAND_CLASS_BASIC_TARIFF_INFO = 0x36;

            public const byte COMMAND_CLASS_BASIC_WINDOW_COVERING = 0x50;

            public const byte COMMAND_CLASS_BASIC = 0x20;

            public const byte COMMAND_CLASS_BATTERY = 0x80;

            public const byte COMMAND_CLASS_CHIMNEY_FAN = 0x2A;

            public const byte COMMAND_CLASS_CLIMATE_CONTROL_SCHEDULE = 0x46;

            public const byte COMMAND_CLASS_CLOCK = 0x81;

            public const byte COMMAND_CLASS_CONFIGURATION = 0x70;

            public const byte COMMAND_CLASS_CONFIGURATION_V2 = 0x70;

            public const byte COMMAND_CLASS_CONTROLLER_REPLICATION = 0x21;

            public const byte COMMAND_CLASS_CRC_16_ENCAP = 0x56;

            public const byte COMMAND_CLASS_DCP_CONFIG = 0x3A;

            public const byte COMMAND_CLASS_DCP_MONITOR = 0x3B;

            public const byte COMMAND_CLASS_DOOR_LOCK_LOGGING = 0x4C;

            public const byte COMMAND_CLASS_DOOR_LOCK = 0x62;

            public const byte COMMAND_CLASS_DOOR_LOCK_V2 = 0x62;

            public const byte COMMAND_CLASS_ENERGY_PRODUCTION = 0x90;

            public const byte COMMAND_CLASS_FIRMWARE_UPDATE_MD = 0x7A;

            public const byte COMMAND_CLASS_FIRMWARE_UPDATE_MD_V2 = 0x7A;

            public const byte COMMAND_CLASS_GEOGRAPHIC_LOCATION = 0x8C;

            public const byte COMMAND_CLASS_GROUPING_NAME = 0x7B;

            public const byte COMMAND_CLASS_HAIL = 0x82;

            public const byte COMMAND_CLASS_HRV_CONTROL = 0x39;

            public const byte COMMAND_CLASS_HRV_STATUS = 0x37;

            public const byte COMMAND_CLASS_INDICATOR = 0x87;

            public const byte COMMAND_CLASS_IP_CONFIGURATION = 0x9A;

            public const byte COMMAND_CLASS_LANGUAGE = 0x89;

            public const byte COMMAND_CLASS_LOCK = 0x76;

            public const byte COMMAND_CLASS_MANUFACTURER_PROPRIETARY = 0x91;

            public const byte COMMAND_CLASS_MANUFACTURER_SPECIFIC = 0x72;

            public const byte COMMAND_CLASS_MANUFACTURER_SPECIFIC_V2 = 0x72;

            public const byte COMMAND_CLASS_MARK = 0xEF;

            public const byte COMMAND_CLASS_METER_PULSE = 0x35;

            public const byte COMMAND_CLASS_METER_TBL_CONFIG = 0x3C;

            public const byte COMMAND_CLASS_METER_TBL_MONITOR = 0x3D;

            public const byte COMMAND_CLASS_METER_TBL_MONITOR_V2 = 0x3D;

            public const byte COMMAND_CLASS_METER_TBL_PUSH = 0x3E;

            public const byte COMMAND_CLASS_METER = 0x32;

            public const byte COMMAND_CLASS_METER_V2 = 0x32;

            public const byte COMMAND_CLASS_METER_V3 = 0x32;

            public const byte COMMAND_CLASS_METER_V4 = 0x32;

            public const byte COMMAND_CLASS_MTP_WINDOW_COVERING = 0x51;

            public const byte COMMAND_CLASS_MULTI_CHANNEL_ASSOCIATION_V2 = 0x8E;

            public const byte COMMAND_CLASS_MULTI_CHANNEL_V2 = 0x60;

            public const byte COMMAND_CLASS_MULTI_CHANNEL_V3 = 0x60;

            public const byte COMMAND_CLASS_MULTI_CMD = 0x8F;

            public const byte COMMAND_CLASS_MULTI_INSTANCE_ASSOCIATION = 0x8E; /*Discontinued*/

            public const byte COMMAND_CLASS_MULTI_INSTANCE = 0x60; /*Discontined*/

            public const byte COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY = 0x52;

            public const byte COMMAND_CLASS_NETWORK_MANAGEMENT_BASIC = 0x4D;

            public const byte COMMAND_CLASS_NETWORK_MANAGEMENT_INCLUSION = 0x34;

            public const byte COMMAND_CLASS_NO_OPERATION = 0x00;

            public const byte COMMAND_CLASS_NODE_NAMING = 0x77;

            public const byte COMMAND_CLASS_NON_INTEROPERABLE = 0xF0;

            public const byte COMMAND_CLASS_POWERLEVEL = 0x73;

            public const byte COMMAND_CLASS_PREPAYMENT_ENCAPSULATION = 0x41;

            public const byte COMMAND_CLASS_PREPAYMENT = 0x3F;

            public const byte COMMAND_CLASS_PROPRIETARY = 0x88;

            public const byte COMMAND_CLASS_PROTECTION = 0x75;

            public const byte COMMAND_CLASS_PROTECTION_V2 = 0x75;

            public const byte COMMAND_CLASS_RATE_TBL_CONFIG = 0x48;

            public const byte COMMAND_CLASS_RATE_TBL_MONITOR = 0x49;

            public const byte COMMAND_CLASS_REMOTE_ASSOCIATION_ACTIVATE = 0x7C;

            public const byte COMMAND_CLASS_REMOTE_ASSOCIATION = 0x7D;

            public const byte COMMAND_CLASS_SCENE_ACTIVATION = 0x2B;

            public const byte COMMAND_CLASS_SCENE_ACTUATOR_CONF = 0x2C;

            public const byte COMMAND_CLASS_SCENE_CONTROLLER_CONF = 0x2D;

            public const byte COMMAND_CLASS_SCHEDULE_ENTRY_LOCK = 0x4E;

            public const byte COMMAND_CLASS_SCHEDULE_ENTRY_LOCK_V2 = 0x4E;

            public const byte COMMAND_CLASS_SCHEDULE_ENTRY_LOCK_V3 = 0x4E;

            public const byte COMMAND_CLASS_SCREEN_ATTRIBUTES = 0x93;

            public const byte COMMAND_CLASS_SCREEN_ATTRIBUTES_V2 = 0x93;

            public const byte COMMAND_CLASS_SCREEN_MD = 0x92;

            public const byte COMMAND_CLASS_SCREEN_MD_V2 = 0x92;

            public const byte COMMAND_CLASS_SECURITY_PANEL_MODE = 0x24;

            public const byte COMMAND_CLASS_SECURITY_PANEL_ZONE_SENSOR = 0x2F;

            public const byte COMMAND_CLASS_SECURITY_PANEL_ZONE = 0x2E;

            public const byte COMMAND_CLASS_SECURITY = 0x98;

            public const byte COMMAND_CLASS_SENSOR_ALARM = 0x9C; /*SDS10963-4 The Sensor Alarm command class can be used to realize Sensor Alarms.*/

            public const byte COMMAND_CLASS_SENSOR_BINARY = 0x30;

            public const byte COMMAND_CLASS_SENSOR_BINARY_V2 = 0x30;

            public const byte COMMAND_CLASS_SENSOR_CONFIGURATION = 0x9E;/*This command class adds the possibility for sensors to act on either a measured value or on a*/

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL = 0x31;

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL_V2 = 0x31;

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL_V3 = 0x31;

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL_V4 = 0x31;

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL_V5 = 0x31;

            public const byte COMMAND_CLASS_SENSOR_MULTILEVEL_V6 = 0x31;

            public const byte COMMAND_CLASS_SILENCE_ALARM = 0x9D;/*SDS10963-4 The Alarm Silence command class can be used to nuisance silence to temporarily disable the sounding*/

            public const byte COMMAND_CLASS_SIMPLE_AV_CONTROL = 0x94;

            public const byte COMMAND_CLASS_SWITCH_ALL = 0x27;

            public const byte COMMAND_CLASS_SWITCH_BINARY = 0x25;

            public const byte COMMAND_CLASS_SWITCH_MULTILEVEL = 0x26;

            public const byte COMMAND_CLASS_SWITCH_MULTILEVEL_V2 = 0x26;

            public const byte COMMAND_CLASS_SWITCH_MULTILEVEL_V3 = 0x26;

            public const byte COMMAND_CLASS_SWITCH_TOGGLE_BINARY = 0x28;

            public const byte COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL = 0x29;

            public const byte COMMAND_CLASS_TARIFF_CONFIG = 0x4A;

            public const byte COMMAND_CLASS_TARIFF_TBL_MONITOR = 0x4B;

            public const byte COMMAND_CLASS_THERMOSTAT_FAN_MODE = 0x44;

            public const byte COMMAND_CLASS_THERMOSTAT_FAN_MODE_V2 = 0x44;

            public const byte COMMAND_CLASS_THERMOSTAT_FAN_MODE_V3 = 0x44;

            public const byte COMMAND_CLASS_THERMOSTAT_FAN_MODE_V4 = 0x44;

            public const byte COMMAND_CLASS_THERMOSTAT_FAN_STATE = 0x45;

            public const byte COMMAND_CLASS_THERMOSTAT_HEATING = 0x38;

            public const byte COMMAND_CLASS_THERMOSTAT_MODE = 0x40;

            public const byte COMMAND_CLASS_THERMOSTAT_MODE_V2 = 0x40;

            public const byte COMMAND_CLASS_THERMOSTAT_MODE_V3 = 0x40;

            public const byte COMMAND_CLASS_THERMOSTAT_OPERATING_STATE = 0x42;

            public const byte COMMAND_CLASS_THERMOSTAT_OPERATING_STATE_V2 = 0x42;

            public const byte COMMAND_CLASS_THERMOSTAT_SETBACK = 0x47;

            public const byte COMMAND_CLASS_THERMOSTAT_SETPOINT = 0x43;

            public const byte COMMAND_CLASS_THERMOSTAT_SETPOINT_V2 = 0x43;

            public const byte COMMAND_CLASS_THERMOSTAT_SETPOINT_V3 = 0x43;

            public const byte COMMAND_CLASS_TIME_PARAMETERS = 0x8B;

            public const byte COMMAND_CLASS_TIME = 0x8A;

            public const byte COMMAND_CLASS_TIME_V2 = 0x8A;

            public const byte COMMAND_CLASS_TRANSPORT_SERVICE = 0x55;

            public const byte COMMAND_CLASS_TRANSPORT_SERVICE_V2 = 0x55;

            public const byte COMMAND_CLASS_USER_CODE = 0x63;

            public const byte COMMAND_CLASS_VERSION = 0x86;

            public const byte COMMAND_CLASS_VERSION_V2 = 0x86;

            public const byte COMMAND_CLASS_WAKE_UP = 0x84;

            public const byte COMMAND_CLASS_WAKE_UP_V2 = 0x84;

            public const byte COMMAND_CLASS_ZIP_6LOWPAN = 0x4F;

            public const byte COMMAND_CLASS_ZIP = 0x23;

            public const byte COMMAND_CLASS_ZIP_V2 = 0x23;

            public const byte COMMAND_CLASS_APPLICATION_CAPABILITY = 0x57;

            public const byte COMMAND_CLASS_COLOR_CONTROL = 0x33;

            public const byte COMMAND_CLASS_COLOR_CONTROL_V2 = 0x33;

            public const byte COMMAND_CLASS_SCHEDULE = 0x53;

            public const byte COMMAND_CLASS_NETWORK_MANAGEMENT_PRIMARY = 0x54;

            public const byte COMMAND_CLASS_ZIP_ND = 0x58;

            public const byte COMMAND_CLASS_ASSOCIATION_GRP_INFO = 0x59;

            public const byte COMMAND_CLASS_DEVICE_RESET_LOCALLY = 0x5A;

            public const byte COMMAND_CLASS_CENTRAL_SCENE = 0x5B;

            public const byte COMMAND_CLASS_IP_ASSOCIATION = 0x5C;

            public const byte COMMAND_CLASS_ANTITHEFT = 0x5D;

            public const byte COMMAND_CLASS_ANTITHEFT_V2 = 0x5D;

            public const byte COMMAND_CLASS_ZWAVEPLUS_INFO = 0x5E; /*SDS11907-3*/

            public const byte COMMAND_CLASS_ZWAVEPLUS_INFO_V2 = 0x5E; /*SDS11907-3*/

            public const byte COMMAND_CLASS_ZIP_GATEWAY = 0x5F;

            public const byte COMMAND_CLASS_ZIP_PORTAL = 0x61;

            public const byte COMMAND_CLASS_APPLIANCE = 0x64;

            public const byte COMMAND_CLASS_DMX = 0x65;

            public const byte COMMAND_CLASS_BARRIER_OPERATOR = 0x66;


            /* Mode parameters to ZW_AddNodeToNetwork */
            public const byte ADD_NODE_ANY = 1;
            public const byte ADD_NODE_CONTROLLER = 2;
            public const byte ADD_NODE_SLAVE = 3;
            public const byte ADD_NODE_EXISTING = 4;
            public const byte ADD_NODE_STOP = 5;
            public const byte ADD_NODE_STOP_FAILED = 6;

            public const byte ADD_NODE_MODE_MASK = 0x0F;
            public const byte ADD_NODE_OPTION_NORMAL_POWER = 0x80;
            public const byte ADD_NODE_OPTION_NETWORK_WIDE = 0x40;

            /* Callback states from ZW_AddNodeToNetwork */
            public const byte ADD_NODE_STATUS_LEARN_READY = 1;
            public const byte ADD_NODE_STATUS_NODE_FOUND = 2;
            public const byte ADD_NODE_STATUS_ADDING_SLAVE = 3;
            public const byte ADD_NODE_STATUS_ADDING_CONTROLLER = 4;
            public const byte ADD_NODE_STATUS_PROTOCOL_DONE = 5;
            public const byte ADD_NODE_STATUS_DONE = 6;
            public const byte ADD_NODE_STATUS_FAILED = 7;
            public const byte ADD_NODE_STATUS_NOT_PRIMARY = 0x23;

            /* Zip Gateway command class commands */
            public const byte ZIP_GATEWAY_VERSION = 0x01;
            public const byte GATEWAY_MODE_SET = 0x01;
            public const byte GATEWAY_MODE_GET = 0x02;
            public const byte GATEWAY_MODE_REPORT = 0x03;
            public const byte GATEWAY_PEER_SET = 0x04;
            public const byte GATEWAY_PEER_GET = 0x05;
            public const byte GATEWAY_PEER_REPORT = 0x06;
            public const byte GATEWAY_LOCK_SET = 0x07;
            public const byte UNSOLICITED_DESTINATION_SET = 0x08;
            public const byte UNSOLICITED_DESTINATION_GET = 0x09;
            public const byte UNSOLICITED_DESTINATION_REPORT = 0x0A;
            public const byte COMMAND_APPLICATION_NODE_INFO_SET = 0x0B;
            public const byte COMMAND_APPLICATION_NODE_INFO_GET = 0x0C;
            public const byte COMMAND_APPLICATION_NODE_INFO_REPORT = 0x0D;
            /* Values used for Gateway Mode Set command */
            public const byte GATEWAY_MODE_SET_STAND_ALONE = 0x01;
            public const byte GATEWAY_MODE_SET_PORTAL = 0x02;
            /* Values used for Gateway Mode Report command */
            public const byte GATEWAY_MODE_REPORT_STAND_ALONE = 0x01;
            public const byte GATEWAY_MODE_REPORT_PORTAL = 0x02;
            /* Values used for Gateway Peer Set command */
            public const byte GATEWAY_PEER_SET_PROPERTIES1_PEER_NAME_LENGTH_MASK = 0x3F;
            public const byte GATEWAY_PEER_SET_PROPERTIES1_RESERVED_MASK = 0xC0;
            public const byte GATEWAY_PEER_SET_PROPERTIES1_RESERVED_SHIFT = 0x06;
            /* Values used for Gateway Peer Report command */
            public const byte GATEWAY_PEER_REPORT_PROPERTIES1_PEER_NAME_LENGTH_MASK = 0x3F;
            public const byte GATEWAY_PEER_REPORT_PROPERTIES1_RESERVED_MASK = 0xC0;
            public const byte GATEWAY_PEER_REPORT_PROPERTIES1_RESERVED_SHIFT = 0x06;
            /* Values used for Gateway Lock Set command */
            public const byte GATEWAY_LOCK_SET_PROPERTIES1_LOCK_BIT_MASK = 0x01;
            public const byte GATEWAY_LOCK_SET_PROPERTIES1_SHOW_BIT_MASK = 0x02;
            public const byte GATEWAY_LOCK_SET_PROPERTIES1_RESERVED_MASK = 0xFC;
            public const byte GATEWAY_LOCK_SET_PROPERTIES1_RESERVED_SHIFT = 0x02;

            /* Zip Portal command class commands */
            public const byte ZIP_PORTAL_VERSION = 0x01;
            public const byte GATEWAY_CONFIGURATION_SET = 0x01;
            public const byte GATEWAY_CONFIGURATION_STATUS = 0x02;
            public const byte GATEWAY_CONFIGURATION_GET = 0x03;
            public const byte GATEWAY_CONFIGURATION_REPORT = 0x04;


            public static DataTable table1 = new DataTable("Gateways");
            public static DataTable table2 = new DataTable("Gateways");
            public static DataTable table3 = new DataTable("SensorData");

        }
        public Form2()
        {
            InitializeComponent();
            IPAddress[] IPS = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in IPS)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    DeltaTIPAddress = ip.ToString();
                }
            }
            table1.Clear();
            var key = new DataColumn[1];
            if (!table1.Columns.Contains("IP"))
            {
                table1.Columns.Add("Select", typeof(bool));
                DataColumn column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "IP";
                key[0] = column;
                table1.Columns.Add(column);
                table1.Columns.Add("Name");
                table1.Columns.Add("MAC");
            }

            table1.PrimaryKey = key;



            table2.Clear();
            if (!table2.Columns.Contains("IP"))
            {
                table2.Columns.Add("IP");
                table2.Columns.Add("Name");
                table2.Columns.Add("MAC");
                table2.Columns.Add("Occupied");
            }
        }

        private void browsebtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose DeltaT Application Folder";
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                selectedpath.Text = fbd.SelectedPath;
            }

        }

        private void ultrabtnnext_Click(object sender, EventArgs e)
        {
            
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (this.ultraTabControl1.SelectedTab.Index == 1)
            {
                this.ultraLabel2.Text = "Fetching any data files associated with your older version of DeltaT...";
                lstgateways.Visible = false;
                while (progressBar1.Value != 100)
                    progressBar1.PerformStep();

                if (progressBar1.Value == 100)
                {
                    if (File.Exists(selectedpath.Text + "\\Resources\\ApplicationData.bin"))
                    {
                        ultraLabel2.Text = "Found some previous data stored. Please press Next to continue migrating the data to new application";
                    }
                }
            }
            if (this.ultraTabControl1.SelectedTab.Index == 2)
            {
                lstgateways.Visible = true;
                ultraDataSource1.LoadFromBinary(selectedpath.Text + "\\Resources\\ApplicationData.bin");
                for (int i = 0; i < ultraDataSource1.Rows.Count; i++)
                {
                    Infragistics.Win.UltraWinDataSource.UltraDataRow row = ultraDataSource1.Rows[i];
                    if (row["Controller1"] != "")
                    {
                        table1.Rows.Add(null, row["Controller1"], row["Controller1Name"], row["Controller1MAC"]);
                    }
                    if (row["Controller2"] != "")
                    {
                        table1.Rows.Add(null, row["Controller2"], row["Controller2Name"], row["Controller2MAC"]);
                    }
                    if (row["Controller3"] != "")
                    {
                        table1.Rows.Add(null, row["Controller3"], row["Controller3Name"], row["Controller3MAC"]);
                    }
                    if (row["Controller4"] != "")
                    {
                        table1.Rows.Add(null, row["Controller4"], row["Controller4Name"], row["Controller4MAC"]);
                    }
                    if (row["Controller5"] != "")
                    {
                        table1.Rows.Add(null, row["Controller5"], row["Controller5Name"], row["Controller5MAC"]);
                    }
                    if (row["Controller6"] != "")
                    {
                        table1.Rows.Add(null, row["Controller6"], row["Controller6Name"], row["Controller6MAC"]);
                    }
                    if (row["Controller7"] != "")
                    {
                        table1.Rows.Add(null, row["Controller7"], row["Controller7Name"], row["Controller7MAC"]);
                    }
                    if (row["Controller8"] != "")
                    {
                        table1.Rows.Add(null, row["Controller8"], row["Controller8Name"], row["Controller8MAC"]);
                    }
                    if (row["Controller9"] != "")
                    {
                        table1.Rows.Add(null, row["Controller9"], row["Controller9Name"], row["Controller9MAC"]);
                    }
                    if (row["Controller10"] != "")
                    {
                        table1.Rows.Add(null, row["Controller10"], row["Controller10Name"], row["Controller10MAC"]);
                    }
                }

                lstgateways.DataSource = table1;
            }
            if (this.ultraTabControl1.SelectedTab.Index == 3)
            {
                var selectedGatewayDetails = GetSelectedGateways();
                ultraGrid1.DataSource = table1;

                try
                {
                    String ipstring, result, host = "";
                    int progresscounter = 1;
                    int i;

                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 4124);
                    byte[] data = { 0x01, 0x23, 0x02, 0x00, 0x40, ++SequenceNumber, 0x00, 0x00, 0x52, 0x03, SequenceNumber, 0xf0, 0x00 };
                    //remoteEP.Port = 4124;
                    byte[] addressBytes = { 0, 0, 0, 0 };
                    addressBytes = IPAddress.Parse(DeltaTIPAddress).GetAddressBytes();

                    int n = Convert.ToInt16(addressBytes[2].ToString());

                    int nn = n - 1;
                    if (n == 0) nn = 0;
                    else if (nn == 0) nn = 1;
                    else if (nn == 253) nn = 253;

                    for (nn = nn; nn < n + 2; nn++)
                    {
                        for (i = 1; i < 255; i++)
                        {
                            remoteEP.Address = IPAddress.Parse(addressBytes[0].ToString() + "."
                                + addressBytes[1].ToString() + "."
                                + nn.ToString() + "."
                                + i.ToString());
                            client.Send(data, data.Length, remoteEP);

                            Thread.Sleep(5);
                            // if ((i % 50 == 0) && MainForm.GatewayCount == 0) MainForm.client.Send(data, data.Length, remoteEP); ;
                        }

                        string controllersfound = GatewayCount.ToString();
                    }



                    //MainForm.client.Close(); // close server 

                    Thread.Sleep(1250);
                }
                catch (Exception ex)
                {
                    UltraMessageBoxManager.Show("Error Looking for Local Gateways!" + Environment.NewLine + ex.Message, "DeltaT Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void ultrabtnnext_Click_1(object sender, EventArgs e)
        {
            if (this.ultraTabControl1.SelectedTab.Index < (this.ultraTabControl1.Tabs.Count - 1))
            {
                // Find the index of the next tab and select it
                this.ultraTabControl1.SelectedTab =
                  this.ultraTabControl1.Tabs[ultraTabControl1.SelectedTab.Index + 1];
            }
        }

        private void lstgateways_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            lstgateways.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
            lstgateways.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

            lstgateways.DisplayLayout.Rows.Band.Columns["Select"].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Left;
            lstgateways.DisplayLayout.Rows.Band.Columns["Select"].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Default;
            lstgateways.DisplayLayout.Rows.Band.Columns["Select"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
        }


        private List<string> GetSelectedGateways()
        {
            List<string> selectedGatewayMACs = new List<string>();
            try
            {
                foreach(UltraGridRow row in lstgateways.DisplayLayout.Rows)
                {
                    bool selected = (bool)row.Cells["Select"].Value;

                    if (selected)
                    {
                        string GatewayMAC = row.Cells["MAC"].Value.ToString();

                        if (!string.IsNullOrEmpty(GatewayMAC))
                        {
                            selectedGatewayMACs.Add(GatewayMAC);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return selectedGatewayMACs;
        }



        public void ReceiveData()
        {
            while (true)
            {
                //    try
                //    {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 4124);

                byte[] UDPdata = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,
             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
             ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
                // byte[] UDPdatatemp;
                byte[] UDPdatatemp = new byte[0];


                try
                {
                    UDPdatatemp = client.Receive(ref anyIP);
                }
                //Perform code that deals with the exception or inform the user what occurred.
                catch (Exception ex)
                {
                }

                //Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "UDP Packet Received:");
                bool KnownDataSource = true;
                string GatewayMac = "";
                int NodeID = 0;

                /*if (UDPdatatemp.Length >= 60)
                { //reset the zipgateway
                    ResetGatewayCount++;
                    new LoggingHelper().WriteGatewayResetLog("Number of Times Gateway Restarted:" + ResetGatewayCount);
                    ResetGateway(anyIP.Address.ToString());
                }*/

                if (UDPdatatemp.Length >= 19)
                {
                    GatewayMac = Convert.ToChar(UDPdatatemp[0]).ToString() + Convert.ToChar(UDPdatatemp[1]).ToString() + Convert.ToChar(UDPdatatemp[2]).ToString() + Convert.ToChar(UDPdatatemp[3]).ToString() +
                                        Convert.ToChar(UDPdatatemp[4]).ToString() + Convert.ToChar(UDPdatatemp[5]).ToString() + Convert.ToChar(UDPdatatemp[6]).ToString() + Convert.ToChar(UDPdatatemp[7]).ToString() +
                                        Convert.ToChar(UDPdatatemp[8]).ToString() + Convert.ToChar(UDPdatatemp[9]).ToString() + Convert.ToChar(UDPdatatemp[10]).ToString() + Convert.ToChar(UDPdatatemp[11]).ToString() +
                                        Convert.ToChar(UDPdatatemp[12]).ToString() + Convert.ToChar(UDPdatatemp[13]).ToString() + Convert.ToChar(UDPdatatemp[14]).ToString() + Convert.ToChar(UDPdatatemp[15]).ToString() + Convert.ToChar(UDPdatatemp[16]).ToString();
                    int tempNodeID = 0;
                    if (UDPdatatemp[18] > 47 && UDPdatatemp[18] < 58)
                    {
                        tempNodeID = UDPdatatemp[18] - 48;
                    }
                    else if (UDPdatatemp[18] > 64 && UDPdatatemp[18] < 71)
                    {
                        tempNodeID = UDPdatatemp[18] - 55;
                    }
                    else if (UDPdatatemp[18] > 96 && UDPdatatemp[18] < 103)
                    {
                        tempNodeID = UDPdatatemp[18] - 87;
                    }
                    else
                    {
                        tempNodeID = 0;
                    }
                    if (UDPdatatemp[17] > 47 && UDPdatatemp[17] < 58)
                    {
                        tempNodeID = tempNodeID + (UDPdatatemp[17] - 48) + ((UDPdatatemp[17] - 48) * 15);
                    }
                    else if (UDPdatatemp[17] > 64 && UDPdatatemp[17] < 71)
                    {
                        tempNodeID = tempNodeID + (UDPdatatemp[17] - 55) + ((UDPdatatemp[17] - 55) * 15);
                    }
                    else if (UDPdatatemp[17] > 96 && UDPdatatemp[17] < 103)
                    {
                        tempNodeID = tempNodeID + (UDPdatatemp[17] - 87) + ((UDPdatatemp[17] - 87) * 15);
                    }

                    NodeID = tempNodeID;
                }


                //test for old gateways
                /*if ((UDPdatatemp[7] == 82 && UDPdatatemp[8] == 4) && anyIP.Port == 4123)
                {
                    //got old Gateway   
                    GatewayCount++;
                    MainForm.table1.Rows.Add(anyIP.Address.ToString(), "Legacy Gateway:" + NetworkUtils.GetMacAddress(System.Net.IPAddress.Parse(anyIP.Address.ToString())).ToString(), "LEGACY");

                    /// doubblecheck
                    if (MainForm.CurrentIPController == "")
                    {
                        MainForm.CurrentIPController = anyIP.Address.ToString();
                    }

                }*/
                //end of test


                Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "UDP Packet Received from Gateway " + GatewayMac + " and Node ID " + NodeID.ToString() + " : ");

                if (UDPdatatemp.Length >= 19)
                {
                    for (int i = 19; i < UDPdatatemp.Length; i++)
                    {
                        UDPdata[i - 19] = UDPdatatemp[i];
                        Console.Write("," + UDPdata[i - 19]);
                    }
                }
                if (KnownDataSource)
                {
                    //byte[] UDPdata = { 0, 0, 0x13, 0x03, 0x2, 0, 136, 0};

                    ZWaveUDP zwFrame = new ZWaveUDP();

                    zwFrame.CommandClass = UDPdata[0];
                    zwFrame.Command = UDPdata[1];
                    zwFrame.properties1 = UDPdata[2];
                    zwFrame.properties2 = UDPdata[3];
                    zwFrame.seqNumber = UDPdata[4];
                    zwFrame.SourceEndPoint = UDPdata[5];
                    zwFrame.DestinationEndPoint = UDPdata[6];
                    zwFrame.Header1 = UDPdata[7];
                    zwFrame.ZWCommandClass = UDPdata[7];
                    zwFrame.ZWCommand = UDPdata[8];
                    zwFrame.ZWpayload1 = UDPdata[9];
                    zwFrame.ZWpayload2 = UDPdata[10];
                    zwFrame.ZWpayload3 = UDPdata[11];
                    zwFrame.ZWpayload4 = UDPdata[12];
                    zwFrame.ZWpayload5 = UDPdata[13];
                    zwFrame.ZWpayload6 = UDPdata[14];
                    if ((zwFrame.ZWCommandClass == ZWConst.COMMAND_CLASS_NETWORK_MANAGEMENT_PROXY) && (zwFrame.ZWCommand == 0x04))
                    {

                        GatewayCount++;
                        /// doubblecheck

                        /// 
                        Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "Gateway Found " + anyIP.Address.ToString() + Environment.NewLine);
                        //v 3.0.1.61MainForm.table1.Rows.Add(anyIP.Address.ToString(), "Gateway" + MainForm.GatewayCount.ToString());

                        if (UDPdatatemp.Length > 56)
                        {
                            Occupied = "";
                            for (int j = 56; j < UDPdatatemp.Length; j++)
                            {
                                Occupied = Occupied + Convert.ToChar(UDPdatatemp[j]);
                            }
                        }
                        //table2.Rows.Add(anyIP.Address.ToString(), "Gateway MAC: " + NetworkUtils.GetMacAddress(System.Net.IPAddress.Parse(anyIP.Address.ToString())).ToString(), Occupied);
                        ultraGrid2.DataSource = table2;
                        Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "Sending ZIP Node GET " + anyIP.Address.ToString() + Environment.NewLine);

                        SendZIPNodeGetCommand(anyIP.Address.ToString(), 1); //let's Get Home ID 


                        Func del = delegate
                        {
                            string DeviceIP = UDPdata[16].ToString() + "."
                                                     + UDPdata[17].ToString() + "."
                                                     + UDPdata[18].ToString() + "."
                                                     + UDPdata[19].ToString();

                            string sHomeID = UDPdata[20].ToString("x2")
                                                     + UDPdata[21].ToString("x2")
                                                     + UDPdata[22].ToString("x2")
                                                     + UDPdata[23].ToString("x2");
                            //formAddSensor.CurrentController = DeviceIP;


                            if ((!anyIP.Address.ToString().Contains("Remote:")) && (!anyIP.Address.ToString().Contains("0.0.")) && (anyIP.Address.ToString().Contains(".")))
                            {
                                DeviceMAC = NetworkUtils.GetMacAddress(System.Net.IPAddress.Parse(anyIP.Address.ToString())).ToString();
                            }
                            else
                            {
                                DeviceMAC = anyIP.Address.ToString().Replace("Remote:", "");
                            }

                            if (DeviceMAC != "not found")
                            {
                                AddSensorIP(UDPdata[3], DeviceIP, DeviceMAC, anyIP.Address.ToString(), sHomeID);
                            }

                        };
                        Invoke(del);
                    }
                    if (zwFrame.CommandClass == ZWConst.COMMAND_CLASS_ZIP_ND && zwFrame.Command == 0x01)
                    {
                        // Node advertisement

                        // if (F_AddingSensor)
                        {
                            Func del = delegate
                            {

                                string DeviceIP = UDPdata[16].ToString() + "."
                                                         + UDPdata[17].ToString() + "."
                                                         + UDPdata[18].ToString() + "."
                                                         + UDPdata[19].ToString();
                                string sHomeID = UDPdata[20].ToString("x2")
                                                         + UDPdata[21].ToString("x2")
                                                         + UDPdata[22].ToString("x2")
                                                         + UDPdata[23].ToString("x2");
                                table2.Rows.Add(anyIP.Address.ToString(), "HomeID: " + sHomeID, DeviceMAC, Occupied);

                                int rows = ultraGrid1.Rows.Count();
                                int newrows = ultraGrid2.Rows.Count();

                                for (int j = 0; j < rows; j++)
                                {
                                    for (int k = 0; k < newrows; k++)
                                    {
                                        if (ultraGrid1.Rows[j].Cells["Name"].Value.ToString() == ultraGrid2.Rows[k].Cells["Name"].Value.ToString())
                                        {
                                            for (int kk = 0; kk < ultraGrid2.Rows.Count; kk++)
                                            {
                                                if (ultraGrid2.Rows[kk].Cells["DeviceMAC"].Value.ToString() == ultraGrid1.Rows[j].Cells["MAC"].Value.ToString().Replace("-", "").ToUpperInvariant())
                                                {
                                                    ultraGrid2.Rows[kk].Cells["DeviceMAC"].Value = ultraGrid2.Rows[k].Cells["MAC"].Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            };

                            Invoke(del);
                        }
                    }
                }
            }
        }

        private int AddSensorIP(byte bNodeID, string DeviceIPAddr, string DeviceMAC, string ControllerIP, string ControllerHomeID)
        {
            bCurrentNodeID = bNodeID;
            currentNodeID = bNodeID.ToString();
            bool sensorfound = false;
            return (0);

        }
        public static void SendZIPNodeGetCommand(string DestinationIPAddress, byte NodeID)
        {
            if ((DestinationIPAddress != "") && (!DestinationIPAddress.StartsWith("0.0.")) && (!DestinationIPAddress.Contains("Remote:")))
            {
                //                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(DestinationIPAddress), 4124); //Otrial
                ///              byte[] ZIPPayload = { 0x58, 0x04, 0x00, NodeID };//Otrial

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(DestinationIPAddress), 4124); // trial
                byte[] ZIPPayload = { 0x01, 0x58, 0x04, 0x00, NodeID };// trial

                if (!remoteEP.ToString().Contains("0.0.0")) client.Send(ZIPPayload, ZIPPayload.Length, remoteEP);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Thread receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }


        public static class NetworkUtils
        {
            // http://www.codeproject.com/KB/IP/host_info_within_network.aspx
            [System.Runtime.InteropServices.DllImport("iphlpapi.dll", ExactSpelling = true)]
            static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

            /// <summary>
            /// Gets the MAC address (<see cref="PhysicalAddress"/>) associated with the specified IP.
            /// </summary>
            /// <param name="ipAddress">The remote IP address.</param>
            /// <returns>The remote machine's MAC address.</returns>
            public static PhysicalAddress GetMacAddress(IPAddress ipAddress)
            {
                Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "ARP,Getting MAC for IPADDRESS: " + ipAddress.ToString());
                const int MacAddressLength = 6;
                int length = MacAddressLength;
                var macBytes = new byte[MacAddressLength];
                SendARP(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0), 0, macBytes, ref length);

                Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + "ARP,MAC: " + macBytes[0].ToString("x02") + "-" + macBytes[1].ToString("x02") + "-" + macBytes[2].ToString("x02") + "-" + macBytes[3].ToString("x02") + "-" + macBytes[4].ToString("x02") + "-" + macBytes[5].ToString("x02"));

                return new PhysicalAddress(macBytes);
            }




        }
    }
}
