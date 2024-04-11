import os
import cv2 as cv
import numpy as np
import random

def aplicar_operacao_aleatoria(pasta_raiz):
    for root, dirs, files in os.walk(pasta_raiz):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg')):
                filepath = os.path.join(root, file)
                
                img = cv.imread(filepath)
                
                if img is not None:
                    kernel_size_dilate = np.random.randint(2, 41)
                    kernel_size_erode = np.random.randint(2, 41) 
                    
                    if random.choice([True, False]):  
                        img = cv.dilate(img, np.ones((kernel_size_dilate, kernel_size_dilate)))
                    else:
                        img = cv.erode(img, np.ones((kernel_size_erode, kernel_size_erode)))
                    
                    cv.imwrite(filepath, img)

aplicar_operacao_aleatoria('alfanumerico')
