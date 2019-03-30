﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using VoidBarcode.iOS;
using VoidBarcode.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoUpdateView), typeof(AutoUpdatePageRenderer))]
namespace VoidBarcode.iOS
{
    public class AutoUpdatePageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            //자동 업데이트
            //*.plist 파일만 https 사이트에 있으면 된다.
            //github에서 [Raw] 버튼을 클릭해서 주소 복사
            var url = new NSUrl("itms-services://?action=download-manifest&url=https://raw.githubusercontent.com/pulmuone/firebase/master/bcwms.plist");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}