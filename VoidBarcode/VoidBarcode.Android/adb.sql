release
<AndroidPackageFormat>aab</AndroidPackageFormat>


msbuild -restore BarcodeScanner.Android.csproj ^
  -t:SignAndroidPackage ^
  -p:Configuration=Release ^
  -p:AndroidKeyStore=True ^
  -p:AndroidPackageFormat=aab ^
  -p:AndroidSigningKeyStore=BCWMS.keystore ^
  -p:AndroidSigningStorePass=bcwms0152 ^
  -p:AndroidSigningKeyAlias=BCWMS ^
  -p:AndroidSigningKeyPass=bcwms0152


msbuild -restore VoidBarcode.Android.csproj ^
-t:SignAndroidPackage ^
-p:Configuration=Release ^
-p:AndroidKeyStore=True ^
-p:AndroidSigningKeyStore=BCWMS.keystore ^
-p:AndroidSigningStorePass=bcwms0152 ^
-p:AndroidSigningKeyAlias=BCWMS ^
-p:AndroidSigningKeyPass=bcwms0152
  
  
  

android:requestLegacyExternalStorage="true"