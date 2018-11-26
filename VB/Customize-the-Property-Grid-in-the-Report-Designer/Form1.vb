Imports DevExpress.Utils.Svg
Imports DevExpress.XtraReports.Design
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace Customize_the_Property_Grid_in_the_Report_Designer
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            AddHandler XtraReport.FilterComponentProperties, AddressOf XtraReport_FilterComponentProperties
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
            Dim report As XtraReport = New XtraReport1()
            Dim designTool As New ReportDesignTool(report)
            AddHandler designTool.DesignRibbonForm.DesignMdiController.DesignPanelLoaded, AddressOf ReportDesigner1_DesignPanelLoaded
            designTool.ShowRibbonDesignerDialog()
        End Sub
        Private Sub XtraReport_FilterComponentProperties(ByVal sender As Object, ByVal e As FilterComponentPropertiesEventArgs)
            Dim propertyDescriptor1 As PropertyDescriptor = TryCast(e.Properties("PaperKind"), PropertyDescriptor)
            If propertyDescriptor1 IsNot Nothing Then
                Dim attributes As New List(Of Attribute)(propertyDescriptor1.Attributes.Cast(Of Attribute)().Where(Function(att) Not (TypeOf att Is PropertyGridTabAttribute)))
                attributes.Add(New PropertyGridTabAttribute("My tab"))
                e.Properties("PaperKind") = TypeDescriptor.CreateProperty(propertyDescriptor1.ComponentType, propertyDescriptor1, attributes.ToArray())
            End If
        End Sub
        Private Sub ReportDesigner1_DesignPanelLoaded(ByVal sender As Object, ByVal e As DevExpress.XtraReports.UserDesigner.DesignerLoadedEventArgs)
            ' Access the tab icon provider
            Dim propertyGridImagesProvider As IPropertyGridIconsProvider = DirectCast(e.DesignerHost.GetService(GetType(IPropertyGridIconsProvider)), IPropertyGridIconsProvider)

            ' Assign an svg icon to the "My tab" tab if it does not have any
            If Not propertyGridImagesProvider.Icons.ContainsKey("My tab") Then
                Dim customTabIcon As SvgImage = SvgImage.FromFile("..\..\CustomTabIcon.svg")
                propertyGridImagesProvider.Icons.Add("My tab", New IconImage(customTabIcon))
            End If
        End Sub
    End Class
End Namespace
