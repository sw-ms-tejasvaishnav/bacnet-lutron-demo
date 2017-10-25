using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BACKnetLutron.BusinessEntities
{
    enum LutronFloorObjectType
    {
        [Display(Name = "Lighting_Level")]
        OBJECT_ANALOG_VALUE = 1,
        [Display(Name = "OBJECT_BINARY_VALUE")]
        OBJECT_BINARY_VALUE = 2,
        [Display(Name = "OBJECT_DEVICE")]
        OBJECT_DEVICE = 3,
        [Display(Name = "OBJECT_NOTIFICATION_CLASS")]
        OBJECT_NOTIFICATION_CLASS = 4,
        [Display(Name = "OBJECT_ALERT_ENROLLMENT")]
        OBJECT_ALERT_ENROLLMENT = 5,
        [Display(Name = "OBJECT_SCHEDULE")]
        OBJECT_SCHEDULE = 6,
    }
}