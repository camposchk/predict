import os
from tensorflow.keras import models, layers, activations, optimizers, utils, losses, initializers, metrics, callbacks, regularizers

epochs = 150 
batch_size = 64 
patience = 10 
learning_rate = 0.001
model_path = 'checkpoints/model.keras'
exists = os.path.exists(model_path)

model = (
    models.load_model(model_path)
    if exists
    else models.Sequential([
        layers.Resizing(32, 32),
        layers.Rescaling(1.0/255),
        layers.Conv2D(64, (3, 3),
            activation = 'relu',
            kernel_initializer = initializers.RandomNormal()
        ),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(128, (3, 3),
            activation = 'relu',
            kernel_initializer = initializers.RandomNormal()
        ),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(256, (3, 3),
            activation = 'relu',
            kernel_initializer = initializers.RandomNormal()
        ),
        layers.MaxPooling2D((2, 2)),
        layers.Flatten(),
        layers.Dropout(0.5),
        layers.Dense(512,
            activation = 'relu',
            kernel_initializer = initializers.RandomNormal()
        ),
        layers.Dropout(0.5),
        layers.Dense(256,
            activation = 'relu',
            kernel_initializer = initializers.RandomNormal()
        ),
        layers.Dense(62,
            activation = 'softmax',
            kernel_initializer = initializers.RandomNormal()
        )
        ])
)

if exists:
    model.summary()
else:
    model.compile(
        optimizer = optimizers.Adam(
            learning_rate = learning_rate
        ),
        loss = losses.SparseCategoricalCrossentropy(),
        metrics = [ 'accuracy' ]
    )

train = utils.image_dataset_from_directory(
    "alfanumerico",
    validation_split= 0.2,
    subset= "training",
    seed= 123,
    shuffle= True,
    image_size= (128, 128),
    batch_size= batch_size
)

test = utils.image_dataset_from_directory(
    "alfanumerico",
    validation_split= 0.2,
    subset= "validation",
    seed= 123,
    shuffle= True,
    image_size= (128, 128),
    batch_size= batch_size
)

model.fit(train,
    epochs = epochs,
    validation_data = test,
    callbacks= [
        callbacks.EarlyStopping(
            monitor = 'val_loss',
            patience = patience,
            verbose = 1
        ),
        callbacks.ModelCheckpoint(
            filepath = model_path,
            save_weights_only = False,
            monitor = 'loss',
            mode = 'min',
            save_best_only = True
        )
    ]
)