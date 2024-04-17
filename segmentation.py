import cv2
import numpy as np
import os

img = cv2.imread('test/imagem.png') 

def flood_fill_segmentation(image, seed_point):
    segmented_img = image.copy()
    mask = np.zeros((image.shape[0] + 2, image.shape[1] + 2), np.uint8)
    new_val = (255, 255, 255)  
    lo_diff = (17, 17, 17)  
    hi_diff = (17, 17, 17)  
    cv2.floodFill(segmented_img, mask, seed_point, new_val, lo_diff, hi_diff)
    return segmented_img

def find_contours(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    thresh = cv2.adaptiveThreshold(gray, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY_INV, 11, 2)
    contours, _ = cv2.findContours(thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    return contours

def draw_rectangles(contour_img, contours):
    for contour in contours:
        area = cv2.contourArea(contour)
        if area > 50:
            x, y, w, h = cv2.boundingRect(contour)
            cv2.rectangle(contour_img, (x, y), (x + w, y + h), (0, 255, 0), 2)

def limpar_arquivos_antigos():
    arquivos_antigos = [arquivo for arquivo in os.listdir('test') if arquivo.startswith('img_cropped')]
    for arquivo in arquivos_antigos:
        os.remove(os.path.join('test', arquivo))

if img is None:
    print('Erro ao carregar a imagem.')
    exit()

segmented_img = flood_fill_segmentation(img, (0, 0))
contours = find_contours(segmented_img)
img_with_rectangles = img.copy()
draw_rectangles(img_with_rectangles, contours)

limpar_arquivos_antigos()

contours_sorted = sorted(contours, key=lambda c: cv2.boundingRect(c)[0])


for i, contour in enumerate(contours_sorted):
    x, y, w, h = cv2.boundingRect(contour)
    
    cropped_img = img[y:y+h, x:x+w]

    border_size = 10
    cropped_img_with_border = cv2.copyMakeBorder(cropped_img, border_size, border_size, border_size, border_size, cv2.BORDER_CONSTANT, value=(255, 255, 255))

    cropped_img_with_border_resized = cv2.resize(cropped_img_with_border, (128, 128))

    cv2.imwrite(f'test/img_cropped{i}.png', cropped_img_with_border_resized)

cv2.imshow('Imagem com retangulos', img_with_rectangles)
cv2.waitKey(0)
cv2.destroyAllWindows()

