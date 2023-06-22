from django import forms

class MenuForm(forms.Form):
    MENU_CHOICES = (
        ('pizza', 'Pizza'),
        ('dania_glowne', 'Dania Główne'),
        ('napoje', 'Napoje'),
        ('przystawki', 'Przystawki')
    )
    menu_type = forms.ChoiceField(choices=MENU_CHOICES, widget=forms.RadioSelect)