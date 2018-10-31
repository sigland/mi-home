﻿using System;
using MiHomeLib.Events;
using Newtonsoft.Json.Linq;

namespace MiHomeLib.Devices
{
    public class AqaraMotionSensor : MiHomeDevice
    {
        public event EventHandler<MotionEventArgs> OnMotion;
        public event EventHandler<NoMotionEventArgs> OnNoMotion;

        public AqaraMotionSensor(string sid) : base(sid, "motion") {}

        public float? Voltage { get; set; }

        public string Status { get; private set; }

        public int NoMotion { get; set; }

        public uint Lux { get; set; }

        public override void ParseData(string command)
        {
            var jObject = JObject.Parse(command);

            if (jObject["status"] != null)
            {
                Status = jObject["status"].ToString();

                if (Status == "motion")
                {
                    MotionDate = DateTime.Now;
                    OnMotion?.Invoke(this, new MotionEventArgs(Lux));
                }
            }

            if (jObject["no_motion"] != null)
            {
                NoMotion = int.Parse(jObject["no_motion"].ToString());
                
                OnNoMotion?.Invoke(this, new NoMotionEventArgs(NoMotion));
            }

            if (jObject["voltage"] != null && float.TryParse(jObject["voltage"].ToString(), out float v))
            {
                Voltage = v / 1000;
            }

            if (jObject["lux"] != null && uint.TryParse(jObject["lux"].ToString(), out uint lux))
            {
                Lux = lux;
            }
        }

        public DateTime? MotionDate { get; private set; }

        public override string ToString()
        {
            return $"Status: {Status}, Voltage: {Voltage}V, NoMotion: {NoMotion}s";
        }

        
    }
}