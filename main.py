# App Name : Seekr

# Kivy for frontend 
import kivy
from kivy.app import App
from kivy.uix.tabbedpanel import TabbedPanel
from kivy.uix.gridlayout import GridLayout
from kivy.uix.label import Label
from kivy.uix.textinput import TextInput
from kivy.uix.image import Image
from kivy.lang import Builder
from kivy.uix.button import Button
from kivy.uix.boxlayout import BoxLayout

#Main App
class SeekrApp(App):
    def build(self):
        return Home()

# Home Page UI
class Home(BoxLayout):
    layout = BoxLayout


if _name_ == '_main_':
    SeekrApp().run()# App Name : Seekr
#
