# Generated by Django 4.2 on 2023-04-20 16:56

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Restauracja', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='product',
            name='price',
            field=models.DecimalField(decimal_places=2, max_digits=6),
        ),
    ]
