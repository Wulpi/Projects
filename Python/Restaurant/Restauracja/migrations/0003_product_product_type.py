# Generated by Django 4.2 on 2023-04-20 18:29

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Restauracja', '0002_alter_product_price'),
    ]

    operations = [
        migrations.AddField(
            model_name='product',
            name='product_type',
            field=models.CharField(choices=[('pizza', 'Pizza'), ('napoj', 'Napój'), ('danie_glowne', 'Danie Główne')], default=2, max_length=15),
            preserve_default=False,
        ),
    ]