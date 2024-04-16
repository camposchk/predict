import cv2 as cv
import numpy as np

# Carrega a imgm
# img = cv.imread('test/inquisicao.png')
# img = cv.imread('test/aqui.jpg')
# img = cv.imread('test/capav.png')

def flood_fill(image, seed_point):
    segmented_img = image.copy()
    mask = np.zeros((image.shape[0] + 2, image.shape[1] + 2), np.uint8)
    new_val = (255, 255, 255) 
    lo_diff = (17, 17, 17)  
    hi_diff = (17, 17, 17)  
    cv.floodFill(segmented_img, mask, seed_point, new_val, lo_diff, hi_diff)
    return segmented_img

def contornos(image):
    gray = cv.cvtColor(image, cv.COLOR_BGR2GRAY)
    thresh = cv.adaptiveThreshold(gray, 255, cv.ADAPTIVE_THRESH_MEAN_C, cv.THRESH_BINARY_INV, 11, 2)
    contours, _ = cv.findContours(thresh, cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
    return contours

def quadrados(contour_img, contours):
    for contour in contours:
        area = cv.contourArea(contour)
        if area > 50:
            x, y, w, h = cv.boundingRect(contour)
            cv.rectangle(contour_img, (x, y), (x + w, y + h), (0, 255, 0), 2)


if img is None:
    print('Erro ao carregar a imagem.')
    exit()

segmented_img = flood_fill(img, (0, 0))
contours = contornos(segmented_img)
img_with_rectangles = img.copy()
quadrados(img_with_rectangles, contours)

cv.imshow('Imagem com retângulos', img_with_rectangles)
cv.waitKey(0)
cv.destroyAllWindows()