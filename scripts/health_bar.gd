extends Control

var heart_full = preload("res://assets/UI/heart_full.png")
var heart_half = preload("res://assets/UI/heart_half.png")
var heart_empty = preload("res://assets/UI/heart_empty.png")

var hearts: Array = []

func _ready():
	$TextureRect.texture = heart_full
